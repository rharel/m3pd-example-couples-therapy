namespace rharel.M3PD.Expectations.Timing
{
    /// <summary>
    /// Enumerates interruption surrender strategies.
    /// </summary>
    public enum InterruptionResponse: int
    {
        /// <summary>
        /// Don't surrender the floor to an interruption by (an)other agent(s).
        /// </summary>
        Ignore = 0,
        /// <summary>
        /// Do surrender the floor to an interruption by (an)other agent(s).
        /// </summary>
        Surrender = 1
    }
}
