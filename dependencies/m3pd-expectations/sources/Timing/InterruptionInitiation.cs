namespace rharel.M3PD.Expectations.Timing
{
    /// <summary>
    /// Enumerates interruption initiation strategies.
    /// </summary>
    public enum InterruptionInitiation: int
    {
        /// <summary>
        /// Don't initiate an interruption.
        /// </summary>
        Avoid = 0,
        /// <summary>
        /// Do initiate an interruption.
        /// </summary>
        Interrupt = 1
    }
}
