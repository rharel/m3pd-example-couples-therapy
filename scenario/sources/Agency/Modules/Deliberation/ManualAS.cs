using rharel.M3PD.Agency.Dialogue_Moves;
using rharel.M3PD.Agency.Modules;

namespace rharel.M3PD.CouplesTherapyExample.Agency
{
    /// <summary>
    /// Implements <see cref="ASModule"/> for the scenario's therapist.
    /// </summary>
    /// <remarks>
    /// This module's move selection is set manually through a the property
    /// <see cref="MoveToSelect"/>. This is so because the therapist is player-
    /// controlled.
    /// </remarks>
    internal sealed class ManualAS: ASModule
    {
        /// <summary>
        /// Gets/sets the dialogue move to select.
        /// </summary>
        public DialogueMove MoveToSelect
        {
            get { return _move_to_select; }
            set
            {
                if (value == null) { value = Idle.Instance; }
                _move_to_select = value;
            }
        }
        /// <summary>
        /// Selects a target dialog move for the system to perform. This 
        /// selects the move specified by <see cref="MoveToSelect"/>.
        /// </summary>
        /// <returns>The value of <see cref="MoveToSelect"/>.</returns>
        public override DialogueMove SelectMove()
        {
            return MoveToSelect;
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(ManualAS)}{{ " +
                   $"{nameof(State)} = {State} }}";
        }

        private DialogueMove _move_to_select = Idle.Instance;
    }
}
