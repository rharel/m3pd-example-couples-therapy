using rharel.Functional;
using rharel.M3PD.Agency.Modules;
using rharel.M3PD.Common.Delegates;
using System;
using System.Collections.Generic;
using static rharel.Functional.Option;

namespace rharel.M3PD.Expectations.Arrangement
{
    /// <summary>
    /// Expects to satisfy a specified child only while a specified condition 
    /// holds.
    /// </summary>
    public sealed class Conditional: Node
    {
        /// <summary>
        /// Creates a new node with the specified identifier and body.
        /// </summary>
        /// <param name="id">The node's identifier.</param>
        /// <param name="body">The node's only child.</param>
        /// <param name="condition">The condition to apply.</param>
        /// <exception cref="ArgumentNullException">
        /// When either <paramref name="id"/>, <paramref name="body"/>, or 
        /// <paramref name="condition"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="id"/> is blank.
        /// </exception>
        public Conditional(string id, Node body, Predicate condition)
            : base(id, new Node[1] { body })
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }
            Body = Children[0];
            Condition = condition;
        }

        /// <summary>
        /// Gets the only child.
        /// </summary>
        public Node Body { get; }
        /// <summary>
        /// Gets this condition predicate.
        /// </summary>
        public Predicate Condition { get; }

        /// <summary>
        /// Gets the index of this node's scope carrier child (if it has any).
        /// </summary>
        public override Optional<int> ScopeCarrierIndex
        {
            get
            {
                if (IsResolved || !Condition.Invoke())
                {
                    return None<int>();
                }
                else
                {
                    return Some(0);
                }
            }
        }

        /// <summary>
        /// Called by <see cref="Process(DialogueEvent)"/> to update this
        /// node's expectations with regard to the processed event.
        /// </summary>
        /// <param name="event">The event to process.</param>
        /// <returns>
        /// <see cref="Resolution.Satisfaction"/> if processing of this event 
        /// has caused all expectations of this node to be satisfied,
        /// <see cref="Resolution.Failure"/> if satisfaction of those
        /// expectations is no longer possible, or 
        /// <see cref="Resolution.Pending"/> otherwise.
        /// </returns>
        protected override Resolution OnProcess(DialogueEvent @event)
        {
            if (!Condition.Invoke()) { return Resolution.Pending; }

            return Body.Process(@event);
        }

        /// <summary>
        /// Computes and adds the expected event set to the specified 
        /// collection.
        /// </summary>
        /// <param name="result">The collection to add events to.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="result"/> is null.
        /// </exception>
        public override void GetExpectedEvents(
            ICollection<DialogueEvent> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }
            if (IsResolved) { return; }

            if (Condition.Invoke()) { Body.GetExpectedEvents(result); }
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(Conditional)}{{ " +
                   $"{nameof(ID)} = '{ID}', " +
                   $"{nameof(Body)} = {Body} }}";
        }
    }
}
