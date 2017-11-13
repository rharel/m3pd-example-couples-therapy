using rharel.M3PD.Common.Collections;
using rharel.M3PD.Common.Delegates;
using System;
using System.Collections.Generic;
using System.Linq;
using IIRule = rharel.M3PD.Expectations.Timing.SocialRule<rharel.M3PD.Expectations.Timing.InterruptionInitiation>;
using IRRule = rharel.M3PD.Expectations.Timing.SocialRule<rharel.M3PD.Expectations.Timing.InterruptionResponse>;

namespace rharel.M3PD.Expectations.Timing
{
    /// <summary>
    /// Bundles interruption rules for both inititation and response.
    /// </summary>
    public sealed class InterruptionRules
    {
        /// <summary>
        /// Rule for global indifference to conflict.
        /// </summary>
        public static readonly InterruptionRules CONFLICT_INDIFFERENCE =
            new InterruptionRules(
                new IIRule[1]
                {
                    new IIRule(  // Always interrupt.
                        precondition: Predicates.Always,
                        affects: Indicators<string>.All,
                        implication: InterruptionInitiation.Interrupt,
                        weight: 1.0f
                    )
                },
                new IRRule[1]
                {
                    new IRRule(  // Never surrender.
                        precondition: Predicates.Always,
                        affects: Indicators<string>.All,
                        implication: InterruptionResponse.Ignore,
                        weight: 1.0f
                    )
                }
            );
        /// <summary>
        /// Rules for global avoidance of conflict.
        /// </summary>
        public static readonly InterruptionRules CONFLICT_AVOIDANCE =
            new InterruptionRules(
                new IIRule[1]    
                {
                    new IIRule(  // Never interrupt.
                        precondition: Predicates.Always,
                        affects: Indicators<string>.All,
                        implication: InterruptionInitiation.Avoid,
                        weight: 1.0f
                    )
                },
                new IRRule[1]
                {
                    new IRRule(  // Always surrender.
                        precondition: Predicates.Always,
                        affects: Indicators<string>.All,
                        implication: InterruptionResponse.Surrender,
                        weight: 1.0f
                    )
                }
            );

        /// <summary>
        /// Creates a new empty rule set.
        /// </summary>
        public InterruptionRules() : this(new IIRule[0], new IRRule[0]) { }
        /// <summary>
        /// Creates a new rule set.
        /// </summary>
        /// <param name="initiation">Initiation rules.</param>
        /// <param name="response">Response rules.</param>
        /// <exception cref="ArgumentNullException">
        /// When either <paramref name="initiation"/> or
        /// <paramref name="response"/> is null.
        /// </exception>
        public InterruptionRules(
            IEnumerable<IIRule> initiation,
            IEnumerable<IRRule> response)
        {
            if (initiation == null)
            {
                throw new ArgumentNullException(nameof(initiation));
            }
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }
            Initiation = new CollectionView<IIRule>(initiation.ToList());
            Response = new CollectionView<IRRule>(response.ToList());
        }
        
        /// <summary>
        /// Gets the interruption initiation rules.
        /// </summary>
        public ImmutableCollection<IIRule> Initiation { get; }
        /// <summary>
        /// Gets the interruption response rules.
        /// </summary>
        public ImmutableCollection<IRRule> Response { get; }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(InterruptionRules)}{{ " +
                   $"{nameof(Initiation)} = {Initiation}, " +
                   $"{nameof(Response)} = {Response} }}";
        }
    }
}
