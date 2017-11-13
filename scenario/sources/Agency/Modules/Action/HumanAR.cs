using rharel.Debug;
using rharel.Functional;
using rharel.M3PD.Agency.Dialogue_Moves;
using rharel.M3PD.Agency.Modules;
using rharel.M3PD.CouplesTherapyExample.Communication;
using rharel.M3PD.CouplesTherapyExample.Random;
using rharel.M3PD.CouplesTherapyExample.Time;
using rharel.M3PD.Expectations.Arrangement;
using rharel.M3PD.Expectations.State;
using System.Collections.Generic;
using static rharel.Functional.Option;
using EBComponents = rharel.M3PD.Expectations.State.CommonComponentIDs;
using ScenarioComponents = rharel.M3PD.CouplesTherapyExample.State.CommonComponentIDs;

namespace rharel.M3PD.CouplesTherapyExample.Agency
{
    /// <summary>
    /// Implements <see cref="ARModule"/> for the scenario's virtual humans.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This module realizes a dialogue move as follows:
    /// 1. It waits a short (randomly sized) duration before commencing output,
    ///    this is done to mimic a natural human pause before speech.
    /// 2. It converts the move to a speech-text realization.
    /// 3. It outputs that realization one word at a time, the interval between
    ///    consecutive words is a parameter supplied during module 
    ///    construction. This text will then be forwarded to the communication
    ///    manager's <see cref="Speech"/> channel.
    /// 4. Once the entire speech text has been output, the move itself is 
    ///    output as well. It will then be forwarded to the communication 
    ///    manger's <see cref="DialogueMove"/> channel.
    /// </para>
    /// <para>
    /// This module require the information state to support the following
    /// components:
    /// 1. Type: <see cref="SocialContext"/>
    ///    Identifier: <see cref="EBComponents.SOCIAL_CONTEXT"/>
    /// 2. Type: <see cref="Clock"/>
    ///    Identifier: <see cref="ScenarioComponents.CLOCK"/>
    /// </para>
    /// </remarks>
    internal sealed class HumanAR: ARModule
    {
        /// <summary>
        /// Creates a new module with the specified output rate.
        /// </summary>
        /// <param name="speech_output_rate">
        /// The time it takes to output a single character of text.
        /// </param>
        /// <remarks>
        /// <paramref name="speech_output_rate"/> must be >= 0.
        /// </remarks>
        public HumanAR(float speech_output_rate = 0.05f)
        {
            Require.IsAtLeast(speech_output_rate, 0);

            _speech_output_rate = speech_output_rate;
        }

        /// <summary>
        /// Gets the word pending realization (only applicable if there is some
        /// move pending realization).
        /// </summary>
        public Optional<string> PendingSpeech
        {
            get
            {
                if (_pending_words.Count > 0)
                {
                    return Some(_pending_words.Peek());
                }
                else
                {
                    return None<string>();
                }
            }
        }
        /// <summary>
        /// Gets the output speech, (if there is any).
        /// </summary>
        public Optional<Speech> OutputSpeech { get; private set; } = (
            None<Speech>()
        );


        /// <summary>
        /// Gets the move pending realization (if there is any).
        /// </summary>
        public Optional<DialogueMove> PendingMove { get; private set; } = ( 
            None<DialogueMove>()
        );
        /// <summary>
        /// Gets this module's output move, if it has any.
        /// </summary>
        public Optional<DialogueMove> OutputMove { get; private set; } = (
            None<DialogueMove>()
        );

        /// <summary>
        /// Sets up this module for operation.
        /// </summary>
        /// <remarks>
        /// This is called once during module initialization.
        /// </remarks>
        public override void Setup()
        {
            var context = State.Get<SocialContext>(
                EBComponents.SOCIAL_CONTEXT
            );
            _self_id = context.SelfID;
            _interaction = context.Interaction;

            _clock = State.Get<Clock>(ScenarioComponents.CLOCK);
            _action_delay_timer = new Timer(_clock);
            _speech_timer = new Timer(_clock);
        }

        /// <summary>
        /// Realizes the specified dialogue move.
        /// </summary>
        /// <param name="move">The move to realize.</param>
        /// <returns>
        /// Information regarding the status of the realization.
        /// </returns>
        public override RealizationStatus RealizeMove(DialogueMove move)
        {
            OutputSpeech = None<Speech>();
            OutputMove = None<DialogueMove>();

            if (move.IsIdle())
            {
                PendingMove = None<DialogueMove>();

                return RealizationStatus.Complete;
            }
            else if (PendingMove.Contains(move))
            {
                return ResumeRealization();
            }
            else
            {
                return BeginRealization(move);
            }
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(HumanAR)}{{ " +
                   $"{nameof(State)} = {State} }}";
        }

        /// <summary>
        /// Begins a new move realization.
        /// </summary>
        /// <param name="move">The move to realize.</param>
        /// <returns>
        /// Information regarding the status of the realization.
        /// </returns>
        private RealizationStatus BeginRealization(DialogueMove move)
        {
            _pending_words.Clear();
            _action_delay_timer.Reset();

            var text = "<Unknown move>";
            move.Properties.ForSome(value =>
            {
                if (value is SpeechTextConvertible convertible)
                {
                    text = convertible.ToSpeechText(move.GetAddressee());
                }
            });
            foreach (var word in text.Split(' '))
            {
                _pending_words.Enqueue(word);
            }

            PendingMove = Some(move);
            SetRandomActionDelay();

            return RealizationStatus.InProgress;
        }
        /// <summary>
        /// Resumes the currently pending move realization.
        /// </summary>
        /// <returns>
        /// Information regarding the status of the realization.
        /// </returns>
        private RealizationStatus ResumeRealization()
        {
            if (_action_delay_timer.Update())
            {
                _action_delay_timer.Reset();

                OutputWord();
            }
            else if (_speech_timer.Update())
            {
                _speech_timer.Reset();

                if (PendingSpeech.IsSome) { OutputWord(); }
                else
                {
                    OutputMove = PendingMove;
                    PendingMove = None<DialogueMove>();

                    return RealizationStatus.Complete;
                }
            }
            return RealizationStatus.InProgress;
        }
        /// <summary>
        /// Outputs the currently pending word.
        /// </summary>
        private void OutputWord()
        {
            string word = _pending_words.Dequeue();
            var speech = new Speech(speaker_id: _self_id, text: word);
            OutputSpeech = Some(speech);

            float output_duration = _speech_output_rate * word.Length;
            _speech_timer.Set(output_duration);
            _speech_timer.Start();
        }

        /// <summary>
        /// Sets the action delay timer to a random value.
        /// </summary>
        private void SetRandomActionDelay()
        {
            float duration = Uniform.Sample(1.0f, 1.5f);
            _action_delay_timer.Set(duration);
            _action_delay_timer.Start();
        }

        private Clock _clock;
        private string _self_id;
        private Node _interaction;

        /// <summary>
        /// The time it takes to output a single character of text.
        /// </summary>
        private readonly float _speech_output_rate;
        private readonly Queue<string> _pending_words = new Queue<string>();

        /// <summary>
        /// Delays first speech output to mimic natural human pause.
        /// </summary>
        private Timer _action_delay_timer;
        /// <summary>
        /// Delays output of the next word by the time it (roughly) took to say
        /// the previous one.
        /// </summary>
        private Timer _speech_timer;
    }
}
