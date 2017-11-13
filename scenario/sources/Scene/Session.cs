using rharel.Debug;
using rharel.M3PD.Agency.Dialogue_Moves;
using rharel.M3PD.Common.Collections;
using rharel.M3PD.Communication.Management;
using rharel.M3PD.CouplesTherapyExample.Communication;
using rharel.M3PD.CouplesTherapyExample.Time;
using rharel.M3PD.Expectations.Arrangement;
using rharel.M3PD.Expectations.State;
using rharel.M3PD.Expectations.Timing;
using System.Collections.Generic;

namespace rharel.M3PD.CouplesTherapyExample.Scene
{
    /// <summary>
    /// Represents a couples-therapy session involving one therapist and a pair 
    /// patient couple.
    /// </summary>
    public class Session
    {
        /// <summary>
        /// Creates a new session with the specified agents.
        /// </summary>
        /// <param name="therapist_id">The therapist's identifier.</param>
        /// <param name="first_patient_id">
        /// The first patient's identifier.
        /// </param>
        /// <param name="second_patient_id">
        /// The second patient's identifier.
        /// </param>
        /// <returns>The newly created session instance.</returns>
        public static Session Create(
            string therapist_id,
            string first_patient_id,
            string second_patient_id)
        {
            Require.IsNotBlank(therapist_id);
            Require.IsNotBlank(first_patient_id);
            Require.IsNotBlank(second_patient_id);

            var therapist = new Therapist(therapist_id);
            var couple = Patient.CreateCouple(
                first_patient_id, 
                second_patient_id
            );
            return new Session(therapist, couple);
        }
        /// <summary>
        /// Creates a mapping between nodes of the sceneario's expectation 
        /// arrangement and sets of interruption rules.
        /// </summary>
        /// <returns>The scenario's interruption rules.</returns>
        internal static Dictionary<string, InterruptionRules> CreateInterruptionRules()
        {
            var result = new Dictionary<string, InterruptionRules>();

            result.Add("greetings", InterruptionRules.CONFLICT_INDIFFERENCE);
            result.Add("counseling", InterruptionRules.CONFLICT_AVOIDANCE);
            result.Add("end of counseling", InterruptionRules.CONFLICT_INDIFFERENCE);
            result.Add("goodbyes", InterruptionRules.CONFLICT_INDIFFERENCE);

            return result;
        }

        /// <summary>
        /// Creates a new session with the specified agents.
        /// </summary>
        /// <param name="therapist">The therapist.</param>
        /// <param name="couple">The patient couple.</param>
        private Session(Therapist therapist, Pair<Patient> couple)
        {
            Require.IsNotNull(therapist);
            Require.AreNotEqual(couple.First, couple.Second);
            Require.AreEqual(couple.First.PartnerID, couple.Second.ID);
            Require.AreEqual(couple.Second.PartnerID, couple.First.ID);

            Therapist = therapist;
            Patients = couple;
            Agents = new CollectionView<VirtualHuman>(
                new VirtualHuman[3] 
                {
                    Therapist,
                    Patients.First,
                    Patients.Second
                }
            );

            _manager = new CommunicationManager.Builder()
                .Support<DialogueMove>()
                .Support<Speech>()
                .Build();

            foreach (var agent in Agents)
            {
                agent.Initialize(this);
                _manager.Engage(agent);
            }

            _interaction = (
                Therapist.Agency
                .State.Get<SocialContext>(CommonComponentIDs.SOCIAL_CONTEXT)
                .Interaction
            );
        }

        /// <summary>
        /// Gets the therapist.
        /// </summary>
        public Therapist Therapist { get; }
        /// <summary>
        /// Gets the patient couple.
        /// </summary>
        public Pair<Patient> Patients { get; }
        /// <summary>
        /// Gets all agents.
        /// </summary>
        public ImmutableCollection<VirtualHuman> Agents { get; }

        /// <summary>
        /// Gets this session's time reference point.
        /// </summary>
        public Clock Clock => _clock;

        /// <summary>
        /// Indicates whether the session has come to an end.
        /// </summary>
        public bool HasEnded => _interaction.IsResolved;

        /// <summary>
        /// Gets supported communication channels.
        /// </summary>
        public ImmutableChannelBatch Channels => _manager.Channels;

        /// <summary>
        /// Advances the simulation in time.
        /// </summary>
        /// <param name="dt">The time duration to advance by.</param>
        public void Step(float dt)
        {
            Require.IsGreaterThan(dt, 0);

            _clock.Tick(dt);
            _manager.Update();
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(Session)}{{ " +
                   $"{nameof(Therapist)} = {Therapist}, " +
                   $"{nameof(Patients)} = {Patients} }}";
        }

        private readonly VariableIncrementClock _clock = (
            new VariableIncrementClock()
        );
        private readonly CommunicationManager _manager;
        private readonly Node _interaction;
    }
}
