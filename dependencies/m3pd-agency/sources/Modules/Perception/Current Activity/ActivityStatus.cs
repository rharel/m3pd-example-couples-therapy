namespace rharel.M3PD.Agency.Modules
{
    /// <summary>
    /// Enumerates possible values of agent activity.
    /// </summary>
    public enum ActivityStatus: int
    {
        /// <summary>
        /// Referring to an idle agent.
        /// </summary>
        Passive = 0,
        /// <summary>
        /// Referring to a non-idle agent.
        /// </summary>
        Active = 1
    }
}
