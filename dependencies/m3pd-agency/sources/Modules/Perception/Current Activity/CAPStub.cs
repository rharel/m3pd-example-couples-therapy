namespace rharel.M3PD.Agency.Modules
{
    /// <summary>
    /// A stub implementation of <see cref="CAPModule"/>.
    /// </summary>
    public sealed class CAPStub: CAPModule
    {
        /// <summary>
        /// Fills in a blank activity report.
        /// </summary>
        /// <param name="report">The report to fill in.</param>
        protected override void ComposeActivityReport(
            CurrentActivity report) { }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>
        /// A human-readable string.
        /// </returns>
        public override string ToString()
        {
            return $"{nameof(CAPStub)}{{ " +
                   $"{nameof(State)} = {State} }}";
        }
    }
}
