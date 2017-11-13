namespace rharel.M3PD.Expectations.State
{
    /// <summary>
    /// Common components used by <see cref="Modules"/>.
    /// </summary>
    public static class CommonComponentIDs
    {
        /// <summary>
        /// Refers to a <see cref="SocialContext"/>.
        /// </summary>
        /// <remarks>
        /// This component is expected to remain constant throughout the 
        /// interaction
        /// </remarks>
        public static readonly string SOCIAL_CONTEXT = (
            $"{typeof(CommonComponentIDs).AssemblyQualifiedName}::" +
            $"{nameof(SocialContext)}"
        );
    }
}
