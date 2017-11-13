namespace rharel.M3PD.Agency.Modules
{
    /// <summary>
    /// This module is responsible for agents in the scene as either passive
    /// (idle) or active (realizing a move).
    /// </summary>
    /// <remarks>
    /// CAP stands for Current Activity Perception.
    /// </remarks>
    public abstract class CAPModule: Module
    {
        /// <summary>
        /// Perceives current agent activity and compiles it into a 
        /// <see cref="CurrentActivity"/>.
        /// </summary>
        public CurrentActivity PerceiveActivity()
        {
            var report = new CurrentActivity();
            ComposeActivityReport(report);

            return report;
        }

        /// <summary>
        /// Fills in an activity report.
        /// </summary>
        /// <param name="report">The report to fill in.</param>
        protected abstract void ComposeActivityReport(CurrentActivity report);

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(CAPModule)}{{ " +
                   $"{nameof(State)} = {State} }}";
        }
    }
}
