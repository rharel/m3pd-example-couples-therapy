namespace rharel.M3PD.Expectations.Arrangement
{
    /// <summary>
    /// Enumerates possible resolution states of an expectation.
    /// </summary>
    public enum Resolution: int
    {
        /// <summary>
        /// The expectation has not been satisfied yet, but could be in the 
        /// future.
        /// </summary>
        Pending = 0,
        /// <summary>
        /// The expectation has not been satisfied, and there is no possibility
        /// for satisfaction in the future.
        /// </summary>
        Failure = -1,
        /// <summary>
        /// The expectation has been satisfied.
        /// </summary>
        Satisfaction = 1
    }
}
