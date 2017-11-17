using rharel.Functional;
using rharel.M3PD.Agency.Modules;
using System;
using System.Collections.Generic;
using static rharel.Functional.Option;

namespace rharel.M3PD.Expectations.Arrangement
{
    /// <summary>
    /// Expects to repeatedly satisfy a specified child.
    /// </summary>
    public sealed class Repetition: Node
    {
        /// <summary>
        /// Creates a new node with the specified identifier and body.
        /// </summary>
        /// <param name="id">The node's identifier.</param>
        /// <param name="body">The node's only child.</param>
        /// <exception cref="ArgumentNullException">
        /// When either <paramref name="id"/> or 
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="id"/> is blank.
        /// </exception>
        public Repetition(string id, Node body)
            : base(id, new Node[1] { body })
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }
            Body = Children[0];
        }

        /// <summary>
        /// Gets the only child.
        /// </summary>
        public Node Body { get; }

        /// <summary>
        /// Gets the index of this node's scope carrier child (if it has any).
        /// </summary>
        public override Optional<int> ScopeCarrierIndex
        {
            get
            {
                if (IsResolved)
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
            Body.Process(@event);

            if (Body.IsFailed || Body.IsPending) { return Body.Resolution; }
            else  // Body is satisfied.
            {
                Body.Reset();
                return Resolution.Pending;
            }
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

            Body.GetExpectedEvents(result);
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(Repetition)}{{ " +
                   $"{nameof(ID)} = '{ID}', " +
                   $"{nameof(Body)} = {Body} }}";
        }
    }
}
