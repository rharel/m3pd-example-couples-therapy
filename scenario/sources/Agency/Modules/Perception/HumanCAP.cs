using rharel.Debug;
using rharel.M3PD.Agency.Modules;
using rharel.M3PD.CouplesTherapyExample.Communication;
using rharel.M3PD.CouplesTherapyExample.Time;
using System.Collections.Generic;
using ScenarioComponents = rharel.M3PD.CouplesTherapyExample.State.CommonComponentIDs;

namespace rharel.M3PD.CouplesTherapyExample.Agency
{
    /// <summary>
    /// Implements <see cref="HumanCAP"/> for the scenario's virtual humans.
    /// </summary>
    /// <remarks>
    /// <para>
    /// An agent is considered active from when we detect its speech and up 
    /// until the end of a specified grace period after it. So, if we detect
    /// speech at t = 0 and have a grace period of 1, then the agent will be
    /// considered active up until t = 1, even if no further speech is produced
    /// by it during that interval.
    /// </para>
    /// <para>
    /// This module require the information state to support the following
    /// components:
    /// 1. Type: <see cref="Clock"/>
    ///    Identifier: <see cref="ScenarioComponents.CLOCK"/>
    /// </para>
    /// </remarks>
    internal sealed class HumanCAP: CAPModule
    {
        /// <summary>
        /// Creates a new module operating with the specified grace period.
        /// See <see cref="GracePeriod"/> for details.
        /// </summary>
        /// <param name="grace_period">The grace period to assume.</param>
        public HumanCAP(float grace_period = 1.0f)
        {
            Require.IsAtLeast(grace_period, 0);

            GracePeriod = grace_period;
        }

        /// <summary>
        /// Gets the grace period duration. 
        /// </summary>
        public float GracePeriod { get; }

        /// <summary>
        /// Gets what will be considered the latest speech data.
        /// </summary>
        public ICollection<Speech> RecentSpeech { get; } = (
            new List<Speech>()
        );

        /// <summary>
        /// Sets up this module for operation.
        /// </summary>
        /// <remarks>
        /// This is called once during module initialization.
        /// </remarks>
        public override void Setup()
        {
            _clock = State.Get<Clock>(ScenarioComponents.CLOCK);
        }

        /// <summary>
        /// Agents are marked as active if they have spoken in the last 
        /// interval of duration <see cref="GracePeriod"/>.
        /// </summary>
        /// <param name="report">The report to fill in.</param>
        protected override void ComposeActivityReport(CurrentActivity report)
        {
            Require.IsNotNull(report);

            foreach (var speech in RecentSpeech)
            {
                _last_activity_time[speech.SpeakerID] = _clock.Time;
            }
            foreach (var agent_id in _last_activity_time.Keys)
            {
                var time_since_last_activity = (
                    _clock.Time - _last_activity_time[agent_id]
                );
                if (time_since_last_activity <= GracePeriod)
                {
                    report.MarkActive(agent_id);
                }
                else
                {
                    report.MarkPassive(agent_id);
                }
            }
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(HumanCAP)}{{ " +
                   $"{nameof(GracePeriod)} = {GracePeriod}, " +
                   $"{nameof(State)} = {State} }}";
        }

        private Clock _clock;

        /// <summary>
        /// Maps an agent's identifier with the last time this agent was 
        /// perceived active.
        /// </summary>
        private readonly Dictionary<string, float> _last_activity_time = ( 
            new Dictionary<string, float>()
        );
    }
}
