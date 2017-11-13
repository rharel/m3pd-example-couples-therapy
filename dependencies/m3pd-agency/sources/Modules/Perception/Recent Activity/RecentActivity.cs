using rharel.M3PD.Agency.Dialogue_Moves;
using rharel.M3PD.Common.Collections;
using System;
using System.Collections.Generic;

namespace rharel.M3PD.Agency.Modules
{
    /// <summary>
    /// The medium through which <see cref="RAPModule"/> reports its output.
    /// </summary>
    public sealed class RecentActivity
    {
        /// <summary>
        /// Creates a new report.
        /// </summary>
        internal RecentActivity()
        {
            Events = new CollectionView<DialogueEvent>(_events);
        }

        /// <summary>
        /// Gets the collection of reported events.
        /// </summary>
        public ImmutableCollection<DialogueEvent> Events { get; }

        /// <summary>
        /// Creates a <see cref="DialogueEvent"/> from the 
        /// specified agent-move pair and adds it to this report.
        /// </summary>
        /// <param name="source_id">
        /// The identifier of the agent who realized the move.
        /// </param>
        /// <param name="move">The move that was realized.</param>
        /// <returns>
        /// True iff the event was added successfully.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// When either <paramref name="source_id"/> or 
        /// <paramref name="move"/> is null.
        /// </exception>
        /// <remarks>
        /// An addition is unsuccessful when either the event has already been
        /// submitted before, or if it contains the idle move (an idle move is
        /// implied if no events were submitted for a specified agent).
        /// </remarks>
        public bool Add(string source_id, DialogueMove move)
        {
            if (source_id == null)
            {
                throw new ArgumentNullException(nameof(source_id));
            }
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }
            return Add(new DialogueEvent(source_id, move));
        }
        /// <summary>
        /// Adds the specified event to this report.
        /// </summary>
        /// <param name="event">The event to add.</param>
        /// <returns>
        /// True iff the event was added successfully.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="event"/> is null.
        /// </exception>
        /// <remarks>
        /// An addition is unsuccessful when either the event has already been
        /// submitted before, or if it contains the idle move (an idle move is
        /// implied if no events were submitted for a specified agent).
        /// </remarks>
        public bool Add(DialogueEvent @event)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }
            return !@event.Move.IsIdle() && _events.Add(@event);
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>
        /// A human-readable string.
        /// </returns>
        public override string ToString()
        {
            return $"{nameof(RecentActivity)}{{ " +
                   $"{nameof(Events)} = {Events} }}";
        }

        private readonly HashSet<DialogueEvent> _events = (
            new HashSet<DialogueEvent>()
        );
    }
}
