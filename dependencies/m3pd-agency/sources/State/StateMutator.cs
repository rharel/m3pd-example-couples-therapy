namespace rharel.M3PD.Agency.State
{
    /// <summary>
    /// Allows mutation of state components.
    /// </summary>
    public interface StateMutator: StateAccessor
    {
        /// <summary>
        /// Sets the component corresponding to the specified type.
        /// </summary>
        /// <typeparam name="T">The type of component to set.</typeparam>
        /// <param name="id">The component's identifier.</param>
        /// <param name="value">The value to assign.</param>
        void Set<T>(string id, T value);
    }
}
