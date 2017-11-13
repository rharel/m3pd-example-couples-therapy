namespace rharel.M3PD.Agency.Modules
{
    /// <summary>
    /// A stub implementation of <see cref="RAPModule"/>.
    /// </summary>
    /// <remarks>
    /// RAP = Recent Activity Perception.
    /// </remarks>
    public sealed class RAPStub: RAPModule
    {
        /// <summary>
        /// Fills in a blank activity report.
        /// </summary>
        /// <param name="report">The report to fill in.</param>
        protected override void ComposeActivityReport(
            RecentActivity report) { }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(RAPStub)}{{ " +
                   $"{nameof(State)} = {State} }}";
        }
    }
}
