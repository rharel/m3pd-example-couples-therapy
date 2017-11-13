using rharel.Functional;
using rharel.M3PD.Agency.Dialogue_Moves;
using rharel.M3PD.Agency.Modules;
using rharel.M3PD.Agency.State;
using System;
using static rharel.Functional.Option;

namespace rharel.M3PD.Agency.System
{
    /// <summary>
    /// This class represents the agency process of one agent, segmented into a 
    /// set of modules sharing an information state object.
    /// </summary>
    public sealed class AgencySystem
    {
        /// <summary>
        /// Creates a new system.
        /// </summary>
        /// <param name="state">The default information state.</param>
        /// <param name="modules">The modules to use.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="state"/> is null.
        /// </exception>
        public AgencySystem(InformationState state, ModuleBundle modules)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            State = state;
            Modules = modules;

            foreach (var module in modules) { module.Initialize(State); }
        }

        /// <summary>
        /// Gets the information state object all modules share.
        /// </summary>
        public InformationState State { get; private set; }
        /// <summary>
        /// Gets the modules that make up this system.
        /// </summary>
        public ModuleBundle Modules { get; private set; }

        /// <summary>
        /// Gets the dialogue move which has completed realization by the 
        /// system in the previous iteration of the perception/action cycle.
        /// </summary>
        /// <remarks>
        /// This refers to complete moves only. If during the last iteration
        /// a move was still in progress this is assigned 
        /// <see cref="None{DialogueMove}"/>.
        /// </remarks>
        public Optional<DialogueMove> RecentMove { get; private set; } = (
            Some<DialogueMove>(Idle.Instance)
        );
        /// <summary>
        /// Gets the target dialogue move.
        /// </summary>
        public DialogueMove TargetMove { get; private set; } = Idle.Instance;
        
        /// <summary>
        /// Indicates whether the system is realizing any move but the idle
        /// move.
        /// </summary>
        public bool IsActive { get; private set; } = false;
        /// <summary>
        /// Indicates whether the system is idle.
        /// </summary>
        public bool IsPassive => !IsActive;
        
        /// <summary>
        /// Performs one iteration of the perception/action cycle.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// When the action selection module selects null as the target move.
        /// </exception>
        public void Step()
        {
            State.RecentActivity = (
                Modules.RecentActivityPerception.PerceiveActivity()
            );
            State.CurrentActivity = (
                Modules.CurrentActivityPerception.PerceiveActivity()
            );
            State.SystemActivity = new SystemActivity(
                RecentMove, TargetMove, IsActive
            );

            Modules.StateUpdate.PerformUpdate(State);

            TargetMove = Modules.ActionSelection.SelectMove();
            if (TargetMove == null)
            {
                throw new InvalidOperationException("Selected move is null.");
            }

            IsActive = (
                TargetMove != Idle.Instance &&
                Modules.ActionTiming.IsValidMoveNow(TargetMove)
            );

            DialogueMove actual_move = IsActive ? TargetMove : Idle.Instance;

            RealizationStatus realization_status = (
                Modules.ActionRealization.RealizeMove(actual_move)
            );
            if (realization_status == RealizationStatus.Complete)
            {
                RecentMove = Some(actual_move);
                IsActive = false;
            }
            else { RecentMove = None<DialogueMove>(); }
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(AgencySystem)}{{ " +
                   $"{nameof(State)} = {State}, " +
                   $"{nameof(Modules)} = {Modules} }}";
        }
    }
}
