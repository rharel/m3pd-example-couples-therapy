using rharel.M3PD.Agency.State;

namespace rharel.M3PD.Agency.Modules
{
    /// <summary>
    /// A stub implementation of the <see cref="SUModule"/>.
    /// </summary>
    public sealed class SUStub: SUModule
    {
        /// <summary>
        /// Leaves the state untouched.
        /// </summary>
        /// <param name="state_mutator">
        /// Allows mutation of state components.
        /// </param>
        public override void PerformUpdate(StateMutator state_mutator) { }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(SUStub)}{{ " +
                   $"{nameof(State)} = {State} }}";
        }
    }
}
