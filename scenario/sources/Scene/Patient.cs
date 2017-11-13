using rharel.Debug;
using rharel.Functional;
using rharel.M3PD.Agency.Dialogue_Moves;
using rharel.M3PD.Agency.Modules;
using rharel.M3PD.Agency.State;
using rharel.M3PD.Agency.System;
using rharel.M3PD.Common.Collections;
using rharel.M3PD.Communication.Management;
using rharel.M3PD.CouplesTherapyExample.Agency;
using rharel.M3PD.CouplesTherapyExample.Communication;
using rharel.M3PD.CouplesTherapyExample.Expectations;
using rharel.M3PD.Expectations.Modules;
using rharel.M3PD.Expectations.State;
using static rharel.M3PD.Communication.Management.CommunicationManager;
using EBComponents = rharel.M3PD.Expectations.State.CommonComponentIDs;
using ScenarioComponents = rharel.M3PD.CouplesTherapyExample.State.CommonComponentIDs;

namespace rharel.M3PD.CouplesTherapyExample.Scene
{
    /// <summary>
    /// Represents an agent being treated during the therapy session.
    /// </summary>
    public sealed class Patient: VirtualHuman
    {
        /// <summary>
        /// Creates a couple from the specified patient identifiers.
        /// </summary>
        /// <param name="first_id">The first patient identifier.</param>
        /// <param name="second_id">The second patient identifier.</param>
        internal static Pair<Patient> CreateCouple(
            string first_id, string second_id)
        {
            Require.IsNotBlank(first_id);
            Require.IsNotBlank(second_id);

            return new Pair<Patient>(
                new Patient(first_id, second_id),
                new Patient(second_id, first_id)
            );
        }

        /// <summary>
        /// Creates a new patient.
        /// </summary>
        /// <param name="id">The agent's identifier.</param>
        /// <param name="partner_id">The agent's partner identifier.</param>
        private Patient(string id, string partner_id): base(id)
        {
            Require.IsNotBlank(partner_id);

            PartnerID = partner_id;
        }

        /// <summary>
        /// Gets this patient's partner identifier.
        /// </summary>
        public string PartnerID { get; }

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
            _CAP = new HumanCAP();
            _AR = new HumanAR();

            var modules = new ModuleBundle.Builder()
                .WithRecentActivityPerceptionBy(_RAP)
                .WithCurrentActivityPerceptionBy(_CAP)
                .WithStateUpdateBy(new HumanSU())
                .WithActionSelectionBy(new ExpectationBasedAS())
                .WithActionTimingBy(new ExpectationBasedAT(
                    Session.CreateInterruptionRules()
                ))
                .WithActionRealizationBy(_AR)
                .Build();

            var state = new InformationState.Builder()
                .WithComponent(ScenarioComponents.CLOCK, session.Clock)
                .WithComponent(
                    EBComponents.SOCIAL_CONTEXT,
                    new SocialContext(ID, ScenarioExpectations.Create(session))
                )
                .Build();

            return new AgencySystem(state, modules);
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
            var recent_speech = _CAP.RecentSpeech;
            recent_speech.Clear();
            foreach (var packet in channels.GetPackets<Speech>())
            {
                recent_speech.Add(packet.Payload);
            }

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
            _AR.OutputMove.ForSome(move => submission.Add(move));
        }

        private HumanRAP _RAP;
        private HumanCAP _CAP;
        private HumanAR _AR;
    }
}
