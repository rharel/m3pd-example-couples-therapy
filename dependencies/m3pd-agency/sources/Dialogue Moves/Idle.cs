namespace rharel.M3PD.Agency.Dialogue_Moves
{
    /// <summary>
    /// The idle move singleton.
    /// </summary>
    public static class Idle
    {
        /// <summary>
        /// Gets the idle move.
        /// </summary>
        public static DialogueMove Instance => _instance;

        private static readonly DialogueMove _instance = (
            DialogueMoves.Create("idle")
        );
    }
}
