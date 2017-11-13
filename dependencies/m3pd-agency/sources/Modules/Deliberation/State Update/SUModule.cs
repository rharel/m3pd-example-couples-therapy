using rharel.M3PD.Agency.State;

namespace rharel.M3PD.Agency.Modules
{
    /// <summary>
    /// This module is responsible for updating of the information state.
    /// </summary>
    /// <remarks>
    /// SU stands for State Update.
    /// </remarks>
    public abstract class SUModule: Module
    {
        /// <summary>
        /// Updates the information state based on perceived activity.
        /// </summary>
        /// <param name="state_mutator">
        /// Allows mutation of state components.
        /// </param>
        public abstract void PerformUpdate(StateMutator state_mutator);

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(SUModule)}{{ " +
                   $"{nameof(State)} = {State} }}";
        }
    }
}
