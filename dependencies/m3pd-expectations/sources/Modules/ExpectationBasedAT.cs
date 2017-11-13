using rharel.Functional;
using rharel.M3PD.Agency.Dialogue_Moves;
using rharel.M3PD.Agency.Modules;
using rharel.M3PD.Expectations.Arrangement;
using rharel.M3PD.Expectations.State;
using rharel.M3PD.Expectations.Timing;
using System;
using System.Collections.Generic;
using IIRule = rharel.M3PD.Expectations.Timing.SocialRule<rharel.M3PD.Expectations.Timing.InterruptionInitiation>;
using IRRule = rharel.M3PD.Expectations.Timing.SocialRule<rharel.M3PD.Expectations.Timing.InterruptionResponse>;

namespace rharel.M3PD.Expectations.Modules
{
    /// <summary>
    /// An implementation of <see cref="Agency.Modules.ATModule"/> that uses
    /// expectation arrangements together with interruption rules to derive 
    /// timing.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Each node in the arrangement is associated with its own set of 
    /// interruption rules. All rules of the scope carrier chain of the 
    /// interaction vote on what the agent's strategy should be regarding both
    /// interruption initiation/response.
    /// </para>
    /// <para>
    /// This module requires the information state to support the following
    /// components:
    /// 1. Type: <see cref="SocialContext"/>
    ///    Identifier: <see cref="CommonComponentIDs.SOCIAL_CONTEXT"/>
    /// </para>
    /// </remarks>
    public sealed class ExpectationBasedAT: Agency.Modules.ATModule
    {
        /// <summary>
        /// Creates a new module with the specified interruption rules.
        /// </summary>
        /// <param name="interruption_rules">
        /// Associates expectation node identifiers to interruption rulesets.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="interruption_rules"/> is null.
        /// </exception>
        public ExpectationBasedAT(
            IEnumerable<KeyValuePair<string, InterruptionRules>> 
            interruption_rules)
        {
            if (interruption_rules == null)
            {
                throw new ArgumentNullException(nameof(interruption_rules));
            }
            foreach (var kv in interruption_rules)
            {
                _interruption_rules.Add(kv.Key, kv.Value);
            }
        }
        /// <summary>
        /// Creates a new module with no interruption rules.
        /// </summary>
        public ExpectationBasedAT(): 
            this(new KeyValuePair<string, InterruptionRules>[0]) { }

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
        public override bool IsValidMoveNow(DialogueMove move)
        {
            if (_interaction.IsResolved) { return false; }

            CurrentActivity current_activity = State.CurrentActivity;

            bool i_am_active = (
                current_activity.ActiveIDs.Contains(_self_id)
            );
            bool floor_is_free = ( 
                current_activity.ActiveIDs.Count == 0
            );
            bool i_am_active_alone = (
                current_activity.ActiveIDs.Count == 1 && i_am_active
            );

            if (floor_is_free || i_am_active_alone) { return true; }

            _active_scope.Clear();
            _interaction.GetScopeCarrierChain(_active_scope);

            if (!i_am_active)
            {
                // We have a move to realize but am not currently active and
                // the floor is occuppied, so we must decide on whether to 
                // initiate an interruption.

                _active_II_rules.Clear();
                foreach (var node in _active_scope)
                {
                    if (_interruption_rules.ContainsKey(node.ID))
                    {
                        _active_II_rules.AddRange(
                            _interruption_rules[node.ID].Initiation
                        );
                    }
                }

                IEnumerable<IIRule> rules;
                if (_active_II_rules.Count == 0)
                {
                    rules = InterruptionRules.CONFLICT_AVOIDANCE.Initiation;
                }
                else
                {
                    rules = _active_II_rules;
                }
                return rules.EvaluateFor(_self_id).MapSomeOr(
                    value => value == InterruptionInitiation.Interrupt,
                    false
                );
            }
            else
            {
                // We are realizing a move to but someone interrupts, so we 
                // must decide on whether to surrender to the interruption.

                _active_IR_rules.Clear();
                foreach (var node in _active_scope)
                {
                    if (_interruption_rules.ContainsKey(node.ID))
                    {
                        _active_IR_rules.AddRange(
                            _interruption_rules[node.ID].Response
                        );
                    }
                }

                IEnumerable<IRRule> rules;
                if (_active_IR_rules.Count == 0)
                {
                    rules = InterruptionRules.CONFLICT_AVOIDANCE.Response;
                }
                else
                {
                    rules = _active_IR_rules;
                }
                return rules.EvaluateFor(_self_id).MapSomeOr(
                    value => value == InterruptionResponse.Ignore,
                    true
                );
            }
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(ExpectationBasedAT)}{{ " +
                   $"{nameof(State)} = {State} }}";
        }

        private string _self_id;
        private Node _interaction;

        private readonly Dictionary<string, InterruptionRules>
            _interruption_rules = (
                new Dictionary<string, InterruptionRules>()
            );

        private readonly ICollection<Node> _active_scope = new List<Node>();

        private readonly List<IIRule> _active_II_rules = new List<IIRule>();
        private readonly List<IRRule> _active_IR_rules = new List<IRRule>();
    }
}
