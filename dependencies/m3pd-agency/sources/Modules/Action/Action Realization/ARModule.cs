using rharel.M3PD.Agency.Dialogue_Moves;

namespace rharel.M3PD.Agency.Modules
{
    /// <summary>
    /// This module is responsible for realizing a specified action.
    /// </summary>
    public abstract class ARModule: Module
    {
        /// <summary>
        /// Realizes the specified dialogue move.
        /// </summary>
        /// <param name="move">The move to realize.</param>
        /// <returns>
        /// Information regarding the status of the realization.
        /// </returns>
        public abstract RealizationStatus RealizeMove(DialogueMove move);

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(ARModule)}{{ " +
                   $"{nameof(State)} = {State} }}";
        }
    }
}
