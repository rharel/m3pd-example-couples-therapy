namespace rharel.M3PD.CouplesTherapyExample.Time
{
    /// <summary>
    /// Represents a read-only view of time.
    /// </summary>
    public interface Clock
    {
        /// <summary>
        /// Gets the current time.
        /// </summary>
        float Time { get; }
        /// <summary>
        /// Gets the latest time increment's size.
        /// </summary>
        float TimeIncrement { get; }
    }
}
