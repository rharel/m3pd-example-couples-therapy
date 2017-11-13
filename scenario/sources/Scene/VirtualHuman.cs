using rharel.Debug;
using rharel.Functional;
using rharel.M3PD.Agency.Dialogue_Moves;
using rharel.M3PD.Agency.Modules;
using rharel.M3PD.Agency.System;
using rharel.M3PD.Communication.Management;
using rharel.M3PD.CouplesTherapyExample.Communication;
using static rharel.M3PD.Communication.Management.CommunicationManager;

namespace rharel.M3PD.CouplesTherapyExample.Scene
{
    /// <summary>
    /// Represents a virtual human agent.
    /// </summary>
    public abstract class VirtualHuman: Agent
    {
        /// <summary>
        /// Creates a new virtual human.
        /// </summary>
        /// <param name="id">The agent's identifier.</param>
        internal protected VirtualHuman(string id)
        {
            Require.IsNotBlank(id);

            ID = id;
        }

        /// <summary>
        /// Gets this agent's identifier.
        /// </summary>
        public string ID { get; }
        /// <summary>
        /// Gets the agency system controlling this agent.
        /// </summary>
        public AgencySystem Agency { get; private set; }

        /// <summary>
        /// Initializes this agent.
        /// </summary>
        /// <param name="session">The session the agent is placed in.</param>
        public void Initialize(Session session)
        {
            Agency = InitializeAgency(session);
        }
        /// <summary>
        /// Initializes the agency system controlling this agent.
        /// </summary>
        /// <param name="session">The session the agent is placed in.</param>
        /// <returns>
        /// The agency system to use for the duration of the interaction.
        /// </returns>
        protected abstract AgencySystem InitializeAgency(Session session);
        
        /// <summary>
        /// Invoked by <see cref="CommunicationManager"/> during the perception
        /// stage of an update iteration.
        /// </summary>
        /// <param name="manager">The manager invoking this method.</param>
        /// <param name="channels">
        /// The data available for perception, organized through channels.
        /// </param>
        public virtual void Perceive(
            CommunicationManager manager, 
            ImmutableChannelBatch channels)
        {
            if (Agency.Modules.RecentActivityPerception is 
                Agency.HumanRAP RAP)
            {
                RAP.RecentEvents.Clear();
                foreach (var packet in channels.GetPackets<DialogueMove>())
                {
                    RAP.RecentEvents.Add(
                        new DialogueEvent(packet.SenderID, packet.Payload)
                    );
                }
            }
            if (Agency.Modules.CurrentActivityPerception is 
                Agency.HumanCAP CAP)
            {
                CAP.RecentSpeech.Clear();
                foreach (var packet in channels.GetPackets<Speech>())
                {
                    CAP.RecentSpeech.Add(packet.Payload);
                }
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
        public virtual void Act(DataSubmission submission)
        {
            if (Agency.Modules.ActionRealization is Agency.HumanAR AR)
            {
                AR.OutputSpeech.ForSome(speech => submission.Add(speech));
                AR.OutputMove.ForSome(move => submission.Add(move));
            }
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>
        /// A human-readable string.
        /// </returns>
        public override string ToString()
        {
            return $"{nameof(VirtualHuman)}{{ " +
                   $"{nameof(ID)} = '{ID}' }}";
        }
    }
}
