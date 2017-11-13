using rharel.M3PD.Agency.Dialogue_Moves;
using rharel.M3PD.Common.Hashing;
using System;

namespace rharel.M3PD.Agency.Modules
{
    /// <summary>
    /// Represents the perceived event of a dialogue move having been fully 
    /// realized.
    /// </summary>
    public sealed class DialogueEvent
    {
        /// <summary>
        /// Creates a new event.
        /// </summary>
        /// <param name="source_id">
        /// The identifier of the agent who realized the move.
        /// </param>
        /// <param name="move">The move that was realized.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="source_id"/> is null.
        /// When <paramref name="move"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="source_id"/> is blank.
        /// </exception>
        public DialogueEvent(string source_id, DialogueMove move)
        {
            if (source_id == null)
            {
                throw new ArgumentNullException(nameof(source_id));
            }
            if (source_id.Trim().Length == 0)
            {
                throw new ArgumentException(nameof(source_id));
            }
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }
            SourceID = source_id;
            Move = move;
        }

        /// <summary>
        /// Gets the identifier of the agent who realized the move.
        /// </summary>
        public string SourceID { get; }

        /// <summary>
        /// Gets the realized move.
        /// </summary>
        public DialogueMove Move { get; }

        /// <summary>
        /// Determines whether the specified object is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>
        /// True iff the specified object is equal to this instance.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is DialogueEvent other)
            {
                return other.SourceID.Equals(SourceID) &&
                       other.Move.Equals(Move);
            }
            return false;
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
            hash = HashCombiner.Hash(hash, SourceID);
            hash = HashCombiner.Hash(hash, Move);

            return hash;
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>
        /// A human-readable string.
        /// </returns>
        public override string ToString()
        {
            return $"{nameof(DialogueEvent)}{{ " +
                   $"{nameof(SourceID)} = '{SourceID}', " +
                   $"{nameof(Move)} = {Move} }}";
        }
    }
}
