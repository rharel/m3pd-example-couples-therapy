using rharel.M3PD.Agency.Modules;
using rharel.M3PD.Agency.State;
using rharel.M3PD.Expectations.Arrangement;
using rharel.M3PD.Expectations.State;
using EBComponents = rharel.M3PD.Expectations.State.CommonComponentIDs;

namespace rharel.M3PD.CouplesTherapyExample.Agency
{
    /// <summary>
    /// Implements <see cref="SUModule"/> for the scenario's virtual humans.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This module makes processes perceived dialogue events through the 
    /// system's <see cref="SocialContext"/>.
    /// </para>
    /// <para>
    /// This module require the information state to support the following
    /// components:
    /// 1. Type: <see cref="SocialContext"/>
    ///    Identifier: <see cref="EBComponents.SOCIAL_CONTEXT"/>
    /// </para>
    /// </remarks>
    internal sealed class HumanSU: SUModule
    {
        /// <summary>
        /// Sets up this module for operation.
        /// </summary>
        /// <remarks>
        /// This is called once during module initialization.
        /// </remarks>
        public override void Setup()
        {
            _interaction = State.Get<SocialContext>(
                EBComponents.SOCIAL_CONTEXT
            ).Interaction;
        }

        /// <summary>
        /// Updates the information state based on perceived activity.
        /// </summary>
        /// <param name="state_mutator">
        /// Allows mutation of state components.
        /// </param>
        public override void PerformUpdate(StateMutator state_mutator)
        {
            foreach (var @event in State.RecentActivity.Events)
            {
                _interaction.Process(@event);
            }
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(HumanSU)}{{ " +
                   $"{nameof(State)} = {State} }}";
        }

        private Node _interaction;
    }
}
