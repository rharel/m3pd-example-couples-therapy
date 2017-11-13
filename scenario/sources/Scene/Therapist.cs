using rharel.Functional;
using rharel.M3PD.Agency.Dialogue_Moves;
using rharel.M3PD.Agency.Modules;
using rharel.M3PD.Agency.State;
using rharel.M3PD.Agency.System;
using rharel.M3PD.Communication.Management;
using rharel.M3PD.CouplesTherapyExample.Agency;
using rharel.M3PD.CouplesTherapyExample.Expectations;
using rharel.M3PD.Expectations.Arrangement;
using rharel.M3PD.Expectations.State;
using System.Collections.Generic;
using System.Linq;
using static rharel.M3PD.Communication.Management.CommunicationManager;
using EBComponents = rharel.M3PD.Expectations.State.CommonComponentIDs;
using ScenarioComponents = rharel.M3PD.CouplesTherapyExample.State.CommonComponentIDs;

namespace rharel.M3PD.CouplesTherapyExample.Scene
{
    /// <summary>
    /// Represents the medical practitioner treating a patient couple.
    /// </summary>
    public sealed class Therapist: VirtualHuman
    {
        /// <summary>
        /// Creates a new therapist.
        /// </summary>
        /// <param name="id">The agent's identifier.</param>
        internal Therapist(string id): base(id) { }

        /// <summary>
        /// Initializes the agency system controlling this agent.
        /// </summary>
        /// <param name="session">The session the agent is placed in.</param>
        /// <returns>
        /// The agency system to use for the duration of the interaction.
        /// </returns>
        protected override AgencySystem InitializeAgency(Session session)
        {
            _RAP = new HumanRAP();
            _AS = new ManualAS();
            _AR = new HumanAR();

            var modules = new ModuleBundle.Builder()
                .WithRecentActivityPerceptionBy(_RAP)
                .WithStateUpdateBy(new HumanSU())
                .WithActionSelectionBy(_AS)
                .WithActionRealizationBy(_AR)
                .Build();

            _interaction = ScenarioExpectations.Create(session);
            var state = new InformationState.Builder()
                .WithComponent(ScenarioComponents.CLOCK, session.Clock)
                .WithComponent(
                    EBComponents.SOCIAL_CONTEXT,
                    new SocialContext(ID, _interaction)
                )
                .Build();

            return new AgencySystem(state, modules);
        }

        /// <summary>
        /// Sets the move to perform.
        /// </summary>
        /// <param name="move">The move to perform.</param>
        public void SetTargetMove(DialogueMove move)
        {
            _AS.MoveToSelect = move;
        }
        /// <summary>
        /// Gets the expected moves to make at this time.
        /// </summary>
        /// <returns>An enumeration of moves.</returns>
        public IEnumerable<DialogueMove> GetSuggestedMoves()
        {
            _expected_events.Clear();
            _interaction.GetExpectedEvents(_expected_events);

            return _expected_events.Where(@event => @event.SourceID == ID)
                                   .Select(@event => @event.Move);
        }

        /// <summary>
        /// Invoked by <see cref="CommunicationManager"/> during the perception
        /// stage of an update iteration.
        /// </summary>
        /// <param name="manager">The manager invoking this method.</param>
        /// <param name="channels">
        /// The data available for perception, organized through channels.
        /// </param>
        public override void Perceive(
            CommunicationManager manager,
            ImmutableChannelBatch channels)
        {
            var recent_events = _RAP.RecentEvents;
            recent_events.Clear();
            foreach (var packet in channels.GetPackets<DialogueMove>())
            {
                recent_events.Add(new DialogueEvent(
                    packet.SenderID, packet.Payload
                ));
            }
        }
        /// <summary>
        /// Invoked by <see cref="CommunicationManager"/> during the action
        /// stage of an update iteration.
        /// </summary>
        /// <param name="submission">
        /// A handle through which agents submit data to the communication
        /// manager.
        /// </param>
        public override void Act(DataSubmission submission)
        {
            Agency.Step();

            _AR.OutputSpeech.ForSome(speech => submission.Add(speech));
            _AR.OutputMove.ForSome(move =>
            {
                submission.Add(move);
                SetTargetMove(Idle.Instance);
            });
        }

        private HumanRAP _RAP;
        private ManualAS _AS;
        private HumanAR _AR;

        private Node _interaction;
        private readonly ICollection<DialogueEvent> _expected_events = (
            new List<DialogueEvent>()
        );
    }
}
