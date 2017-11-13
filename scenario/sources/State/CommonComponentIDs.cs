namespace rharel.M3PD.CouplesTherapyExample.State
{
    /// <summary>
    /// Common components used by <see cref="Agency"/> modules.
    /// </summary>
    public static class CommonComponentIDs
    {
        /// <summary>
        /// Refers to the scenario's <see cref="Time.Clock"/>.
        /// </summary>
        /// <remarks>
        /// This component is expected to remain constant throughout the 
        /// interaction
        /// </remarks>
        public static readonly string CLOCK = (
            $"{typeof(CommonComponentIDs).AssemblyQualifiedName}::" +
            $"{nameof(Time.Clock)}"
        );
    }
}
