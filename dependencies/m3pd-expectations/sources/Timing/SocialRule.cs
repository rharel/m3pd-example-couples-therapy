using rharel.M3PD.Common.Delegates;
using rharel.M3PD.Common.Hashing;
using System;

namespace rharel.M3PD.Expectations.Timing
{
    /// <summary>
    /// Represents a social rule. Social rules weigh-in on some decision
    /// problem and imply a response. 
    /// </summary>
    /// <remarks>
    /// A rule contains the following components:
    ///     1. A precondition determining when it applies.
    ///     2. An indicator function determining onto which agents it applies.
    ///     3. The response the rule implies to the decision problem.
    ///     4. A numeric weight, indicating how dominating this rule is
    ///        relative to others.
    /// </remarks>
    /// <typeparam name="T">
    /// The type of implication the rule entails.
    /// </typeparam>
    public sealed class SocialRule<T>
    {
        /// <summary>
        /// Creates a new rule.
        /// </summary>
        /// <param name="precondition">
        /// The precondition required to activate this rule.
        /// </param>
        /// <param name="affects">
        /// An indicator of the agents this rule affects.
        /// </param>
        /// <param name="implication">This rule's implication.</param>
        /// <param name="weight">This rule's weight.</param>
        /// <exception cref="ArgumentNullException">
        /// When either <paramref name="precondition"/> or
        /// <paramref name="affects" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="weight"/> is negative.
        /// </exception>
        public SocialRule(
            Predicate precondition, 
            Indicator<string> affects,
            T implication,
            float weight = 1.0f)
        {
            if (precondition == null)
            {
                throw new ArgumentNullException(nameof(precondition));
            }
            if (affects == null)
            {
                throw new ArgumentNullException(nameof(affects));
            }
            if (weight < 0)
            {
                throw new ArgumentException(nameof(weight));
            }

            _precondition = precondition;
            _affected_indicator = affects;
            Implication = implication;
            Weight = weight;
        }

        /// <summary>
        /// Gets this rule's implication.
        /// </summary>
        public T Implication { get; }
        /// <summary>
        /// Gets this rule's weight.
        /// </summary>
        public float Weight { get; }

        /// <summary>
        /// Determines whether this rule is relevant at this time, i.e. if its
        /// precondition is satisfied.
        /// </summary>
        /// <returns>True iff this rule is relevant at this time.</returns>
        public bool IsRelevant()
        {
            return _precondition.Invoke();
        }
        /// <summary>
        /// Determines whether the specified agent is affected by this rule.
        /// </summary>
        /// <param name="id">The agent identifier to test.</param>
        /// <returns>
        /// True if the specified agent is affected by this rule, otherwise 
        /// false.
        /// </returns>
        public bool IsAffecting(string id)
        {
            return _affected_indicator.Invoke(id);
        }

        /// <summary>
        /// Determines whether the specified object is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>
        /// True if the specified object is equal to this instance; 
        /// otherwise false.
        /// </returns>
        public override bool Equals(object obj)
        {
            var other = obj as SocialRule<T>;

            if (ReferenceEquals(other, null)) { return false; }
            if (ReferenceEquals(other, this)) { return true; }

            return other._precondition.Equals(_precondition) &&
                   other._affected_indicator.Equals(_affected_indicator) &&
                   Equals(other.Implication, Implication) &&
                   other.Weight.Equals(Weight);
        }
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing 
        /// algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            int hash = HashCombiner.Initialize();
            hash = HashCombiner.Hash(hash, _precondition);
            hash = HashCombiner.Hash(hash, _affected_indicator);
            hash = HashCombiner.Hash(hash, Implication);
            hash = HashCombiner.Hash(hash, Weight);

            return hash;
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(SocialRule<T>)}{{ " +
                   $"{nameof(Implication)} = {Implication}, " +
                   $"{nameof(Weight)} = {Weight} }}";
        }

        private readonly Predicate _precondition;
        private readonly Indicator<string> _affected_indicator;
    }
}
