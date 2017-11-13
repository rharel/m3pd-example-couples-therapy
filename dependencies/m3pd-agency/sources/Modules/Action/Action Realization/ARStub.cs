using rharel.M3PD.Agency.Dialogue_Moves;

namespace rharel.M3PD.Agency.Modules
{
    /// <summary>
    /// A stub implementation of <see cref="ARModule"/>.
    /// </summary>
    public sealed class ARStub: ARModule
    {
        /// <summary>
        /// Realizes the specified dialogue move.
        /// </summary>
        /// <param name="move">The move to realize.</param>
        /// <returns><see cref="RealizationStatus.Complete"/></returns>
        public override RealizationStatus RealizeMove(DialogueMove move)
        {
            return RealizationStatus.Complete;
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(ARStub)}{{ " +
                   $"{nameof(State)} = {State} }}";
        }
    }
}
