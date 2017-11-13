using rharel.M3PD.Agency.Modules;
using System.Collections.Generic;

namespace rharel.M3PD.CouplesTherapyExample.Agency
{
    /// <summary>
    /// Implements <see cref="RAPModule"/> for the 
    /// scenario's virtual humans.
    /// </summary>
    internal sealed class HumanRAP: RAPModule
    {
        /// <summary>
        /// Gets what will be considered the latest dialogue events.
        /// </summary>
        public ICollection<DialogueEvent> RecentEvents { get; } = (
            new List<DialogueEvent>()
        );

        /// <summary>
        /// Fills in an activity report.
        /// </summary>
        /// <param name="report">The report to fill in.</param>
        protected override void ComposeActivityReport(RecentActivity report)
        {
            foreach (var @event in RecentEvents) { report.Add(@event); }
        }
    }
}
