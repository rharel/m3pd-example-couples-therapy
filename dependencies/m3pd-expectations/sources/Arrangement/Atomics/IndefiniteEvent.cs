using rharel.M3PD.Agency.Modules;
using System;
using System.Collections.Generic;

namespace rharel.M3PD.Expectations.Arrangement
{
    /// <summary>
    /// Expects a specified event indefinitely.
    /// </summary>
    public sealed class IndefiniteEvent: Node
    {
        /// <summary>
        /// Creates a new node with the specified identifier and expecting the 
        /// specified event.
        /// </summary>
        /// <param name="id">The node's identifier.</param>
        /// <param name="event">The expected event.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="id"/> is null.
        /// When <paramref name="event"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="id"/> is blank.
        /// </exception>
        public IndefiniteEvent(string id, DialogueEvent @event): base(id)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }
            Event = @event;
            Reset();
        }

        /// <summary>
        /// Gets the expected event.
        /// </summary>
        public DialogueEvent Event { get; }

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
            if (@event.Equals(Event))
            {
                return Resolution.Satisfaction;
            }
            else
            {
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

            result.Add(Event);
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(IndefiniteEvent)}{{ " +
                   $"{nameof(ID)} = '{ID}', " +
                   $"{nameof(Event)} = {Event}, " +
                   $"{nameof(Resolution)} = {Resolution} }}";
        }
    }
}
