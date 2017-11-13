namespace rharel.M3PD.Agency.Modules
{
    /// <summary>
    /// Enumerates possible values of an action's realization status.
    /// </summary>
    public enum RealizationStatus: int
    {
        /// <summary>
        /// Referring to a completed realization.
        /// </summary>
        Complete = 0,
        /// <summary>
        /// Referring to a realization in progress.
        /// </summary>
        InProgress = 1
    }
}
