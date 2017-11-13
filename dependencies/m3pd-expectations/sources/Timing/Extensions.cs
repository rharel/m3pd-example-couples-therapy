using rharel.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using static rharel.Functional.Option;

namespace rharel.M3PD.Expectations.Timing
{
    /// <summary>
    /// Extension methods for social rules.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Evaluates an enumeration of rules for the specified agent at this 
        /// time.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the implication the rules derive.
        /// </typeparam>
        /// <param name="rules">The enumerable to evaluate.</param>
        /// <param name="id">
        /// The identifier of the agent against which the rules should be 
        /// evaluated.
        /// </param>
        /// <returns>
        /// The implication supported most heavily, or <see cref="None"/> if
        /// no rules were relevant for <paramref name="id"/>.
        /// </returns>
        /// <remarks>
        /// The rules each vote for their own implication with their weight.
        /// The implication with the highest total weight is the one that will 
        /// be returned. In case of a tie, the rule appearing last in the 
        /// enumeration wins.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="id"/> is null.
        /// </exception>
        public static Optional<T> EvaluateFor<T>(
            this IEnumerable<SocialRule<T>> rules, 
            string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var ballot = new Dictionary<T, float>();
            foreach (var rule in rules)
            {
                if (!rule.IsRelevant() || !rule.IsAffecting(id))
                {
                    continue;
                }
                if (!ballot.ContainsKey(rule.Implication))
                {
                    ballot.Add(rule.Implication, 0.0f);
                }
                ballot[rule.Implication] += rule.Weight;
            }
            if (ballot.Count == 0) { return None<T>(); }
            else
            {
                return Some(ballot.Aggregate(
                    (a, b) => a.Value > b.Value ? a : b).Key
                );
            }
        }
    }
}
