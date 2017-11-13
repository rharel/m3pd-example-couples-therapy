using rharel.M3PD.Agency.Modules;
using System;
using System.Collections.Generic;

namespace rharel.M3PD.Expectations.Arrangement
{
    /// <summary>
    /// Expects satisfaction of at least one child.
    /// </summary>
    public sealed class Disjunction: Node
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
        public Disjunction(string id, IEnumerable<Node> children)
            : base(id, children)
        {
            if (Children.Count < 2)
            {
                throw new ArgumentException(nameof(children));
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
            bool all_children_are_failed = true;
            foreach (var child in Children)
            {
                if (!child.IsResolved) { child.Process(@event); }

                switch (child.Resolution)
                {
                    case Resolution.Satisfaction:
                    {
                        return Resolution.Satisfaction;
                    }
                    case Resolution.Pending:
                    {
                        all_children_are_failed = false;
                        break;
                    }
                }
            }
            return all_children_are_failed ?
                   Resolution.Failure :
                   Resolution.Pending;
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

            foreach (var child in Children)
            {
                if (!child.IsResolved) { child.GetExpectedEvents(result); }
            }
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(Disjunction)}{{ " +
                   $"{nameof(ID)} = '{ID}', " +
                   $"{nameof(Children)} = {Children} }}";
        }
    }
}
