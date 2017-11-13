using rharel.M3PD.Agency.Dialogue_Moves;

namespace rharel.M3PD.Agency.Modules
{
    /// <summary>
    /// This module is responsible for timing the realization of action.
    /// </summary>
    /// <remarks>
    /// AT stands for Action Timing.
    /// </remarks>
    public abstract class ATModule: Module
    {
        /// <summary>
        /// Determines whether now is a valid time to realize the specified 
        /// dialogue move.
        /// </summary>
        /// <param name="move">The desired move to make.</param>
        /// <returns>
        /// True iff now is a valid time to realize the specified move.
        /// </returns>
        /// <remarks>
        /// This will never be given the idle move as an argument.
        /// </remarks>
        public abstract bool IsValidMoveNow(DialogueMove move);

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(ATModule)}{{ " +
                   $"{nameof(State)} = {State} }}";
        }
    }
}
