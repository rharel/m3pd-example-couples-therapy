using rharel.Functional;
using rharel.M3PD.Agency.Dialogue_Moves;
using rharel.M3PD.Agency.Modules;
using rharel.M3PD.Agency.State;
using rharel.M3PD.Expectations.Arrangement;
using rharel.M3PD.Expectations.State;
using System;
using System.Collections.Generic;
using static rharel.Functional.Option;

namespace rharel.M3PD.Expectations.Modules
{
    /// <summary>
    /// An implementation of <see cref="Agency.Modules.ASModule"/> that uses
    /// expectation arrangements to derive action.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A list of candidate moves is derived from an interaction expectation 
    /// arrangement, and then a user-supplied method selects the winning 
    /// candidate.
    /// </para>
    /// <para>
    /// This module requires the information state to support the following
    /// components:
    /// 1. Type: <see cref="SocialContext"/>
    ///    Identifier: <see cref="CommonComponentIDs.SOCIAL_CONTEXT"/>
    /// </para>
    /// </remarks>
    public sealed class ExpectationBasedAS: Agency.Modules.ASModule
    {
        /// <summary>
        /// Selects of one winner out of a set of candidate moves.
        /// </summary>
        /// <param name="candidates">
        /// The candidate dialogue move list.
        /// </param>
        /// <param name="state">The current information state.</param>
        /// <returns>
        /// The index of the winning move in the candidate list, or 
        /// <see cref="None"/> to select the idle move.
        /// </returns>
        public delegate Optional<int> SelectionMethod(
            List<DialogueMove> candidates, 
            StateAccessor state
        );

        /// <summary>
        /// Creates a new module using the specfied winner candidate selector.
        /// </summary>
        /// <param name="candidate_selector">
        /// The selection method to use when choosing a winner from the 
        /// candidate move list.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="candidate_selector"/> is null.
        /// </exception>
        public ExpectationBasedAS(SelectionMethod candidate_selector)
        {
            if (candidate_selector == null)
            {
                throw new ArgumentNullException(nameof(candidate_selector));
            }
            CandidateSelector = candidate_selector;
        }
        /// <summary>
        /// Creates a new module with a default winner candidate selector.
        /// </summary>
        /// <remarks>
        /// The default selector always selects the first candidate as the 
        /// winner.
        /// </remarks>
        public ExpectationBasedAS(): this((candidates, state) => Some(0)) { }

        /// <summary>
        /// Gets the method used to select a winning candidate move.
        /// </summary>
        public SelectionMethod CandidateSelector { get; }

        /// <summary>
        /// Sets up this module for operation.
        /// </summary>
        /// <remarks>
        /// This is called once during module initialization.
        /// </remarks>
        public override void Setup()
        {
            var context = State.Get<SocialContext>(
                CommonComponentIDs.SOCIAL_CONTEXT
            );
            _self_id = context.SelfID;
            _interaction = context.Interaction;
        }

        /// <summary>
        /// Selects a target move for the system to perform.
        /// </summary>
        /// <returns>
        /// A move derived from the state's 
        /// <see cref="SocialContext.Interaction"/> expectation arrangement.
        /// </returns>
        public override DialogueMove SelectMove()
        {
            if (_interaction.IsResolved) { return Idle.Instance; }

            _expected_events.Clear();
            _interaction.GetExpectedEvents(_expected_events);

            _candidates.Clear();
            foreach (var @event in _expected_events)
            {
                if (@event.SourceID == _self_id)
                {
                    _candidates.Add(@event.Move);
                }
            }
            if (_candidates.Count == 0) { return Idle.Instance; }
            else if (_candidates.Count == 1) { return _candidates[0]; }
            else
            {
                return CandidateSelector.Invoke(_candidates, State).MapSomeOr(
                    index => _candidates[index],
                    Idle.Instance
                );
            }
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(ExpectationBasedAS)}{{ " +
                   $"{nameof(State)} = {State} }}";
        }

        private string _self_id;
        private Node _interaction;

        private readonly ICollection<DialogueEvent> _expected_events = (
            new List<DialogueEvent>()
        );
        private readonly List<DialogueMove> _candidates = (
            new List<DialogueMove>()
        );
    }
}
