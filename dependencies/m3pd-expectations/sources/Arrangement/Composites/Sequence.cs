using rharel.Functional;
using rharel.M3PD.Agency.Modules;
using System;
using System.Collections.Generic;
using static rharel.Functional.Option;

namespace rharel.M3PD.Expectations.Arrangement
{
    /// <summary>
    /// Expects satisfaction of all children in sequence.
    /// </summary>
    public sealed class Sequence: Node
    {
        /// <summary>
        /// Creates a new node with the specified identifier and children.
        /// </summary>
        /// <param name="id">The node's identifier.</param>
        /// <param name="children">The node's children.</param>
        /// <exception cref="ArgumentNullException">
        /// When either <paramref name="id"/> or 
        /// <paramref name="children"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="id"/> is blank.
        /// When <paramref name="children"/> contains less than two members.
        /// </exception>
        public Sequence(string id, IEnumerable<Node> children): 
            base(id, children)
        {
            if (Children.Count < 2)
            {
                throw new ArgumentException(nameof(children));
            }
        }

        /// <summary>
        /// Gets the index of the active child.
        /// </summary>
        public int ActiveChildIndex { get; private set; } = 0;
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
                    return Some(ActiveChildIndex);
                }
            }
        }

        /// <summary>
        /// Called by <see cref="Node.Reset"/> to reset this node's 
        /// expectations back to their initial state.
        /// </summary>
        protected override void OnReset()
        {
            ActiveChildIndex = 0;
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
            ActiveChild.Process(@event);

            if (ActiveChild.IsFailed || ActiveChild.IsPending)
            {
                return ActiveChild.Resolution;
            }
            else  // Child is satisfied.
            {
                ActiveChildIndex += 1;
                while (ActiveChildIndex < Children.Count &&
                       ActiveChild.IsSatisfied)
                {
                    ActiveChildIndex += 1;
                }
                if (ActiveChildIndex == Children.Count)
                {
                    return Resolution.Satisfaction;
                }
                else if (ActiveChild.IsFailed)
                {
                    return Resolution.Failure;
                }
                else { return Resolution.Pending; }
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

            ActiveChild.GetExpectedEvents(result);
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(Sequence)}{{ " +
                   $"{nameof(ID)} = '{ID}', " +
                   $"{nameof(ActiveChildIndex)} = {ActiveChildIndex}, " +
                   $"{nameof(Children)} = {Children} }}";
        }

        /// <summary>
        /// Gets the current child to be satisfied.
        /// </summary>
        private Node ActiveChild => Children[ActiveChildIndex];
    }
}
