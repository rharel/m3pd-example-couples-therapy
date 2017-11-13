using rharel.M3PD.Agency.Dialogue_Moves;

namespace rharel.M3PD.Agency.Modules
{
    /// <summary>
    /// A stub implementation of <see cref="ATModule"/>.
    /// </summary>
    public sealed class ATStub: ATModule
    {
        /// <summary>
        /// Determines whether now is a valid time to realize the specified 
        /// dialogue move.
        /// </summary>
        /// <param name="move">The desired move to make.</param>
        /// <returns>True,</returns>
        public override bool IsValidMoveNow(DialogueMove move)
        {
            return true;
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(ATStub)}{{ " +
                   $"{nameof(State)} = {State} }}";
        }
    }
}
