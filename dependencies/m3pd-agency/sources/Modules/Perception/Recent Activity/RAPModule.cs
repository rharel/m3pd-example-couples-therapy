namespace rharel.M3PD.Agency.Modules
{
    /// <summary>
    /// This module is responsible for perceiving dialogue moves that have 
    /// been realized recently.
    /// </summary>
    /// <remarks>
    /// RAP stands for Recent Activity Perception.
    /// </remarks>
    public abstract class RAPModule: Module
    {
        /// <summary>
        /// Perceives recently-realized dialogue moves and compiles them into a
        /// <see cref="RecentActivity"/>.
        /// </summary>
        internal RecentActivity PerceiveActivity()
        {
            var report = new RecentActivity();
            ComposeActivityReport(report);
            return report;
        }

        /// <summary>
        /// Fills in an activity report.
        /// </summary>
        /// <param name="report">The report to fill in.</param>
        protected abstract void ComposeActivityReport(RecentActivity report);

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(RAPModule)}{{ " +
                   $"{nameof(State)} = {State} }}";
        }
    }
}
