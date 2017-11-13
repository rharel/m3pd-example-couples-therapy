using rharel.M3PD.Agency.Dialogue_Moves;

namespace rharel.M3PD.Agency.Modules
{
    /// <summary>
    /// This module is responsible for selecting the system's target move to 
    /// realize.
    /// </summary>
    /// <remarks>
    /// AS stands for Action Selection.
    /// </remarks>
    public abstract class ASModule: Module
    {
        /// <summary>
        /// Selects a target move for the system to perform.
        /// </summary>
        /// <returns>The selected move.</returns>
        public abstract DialogueMove SelectMove();

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(ASModule)}{{ " +
                   $"{nameof(State)} = {State} }}";
        }
    }
}
