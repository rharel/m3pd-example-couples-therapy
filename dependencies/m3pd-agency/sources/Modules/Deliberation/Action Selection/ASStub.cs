using rharel.M3PD.Agency.Dialogue_Moves;

namespace rharel.M3PD.Agency.Modules
{
    /// <summary>
    /// A stub implementation of <see cref="ASModule"/>.
    /// </summary>
    public sealed class ASStub: ASModule
    {
        /// <summary>
        /// Selects a target move for the system to perform.
        /// </summary>
        /// <returns>The idle move.</returns>
        public override DialogueMove SelectMove()
        {
            return Idle.Instance;
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(ASStub)}{{ " +
                   $"{nameof(State)} = {State} }}";
        }
    }
}
