using rharel.Debug;
using rharel.Functional;
using rharel.M3PD.Agency.Dialogue_Moves;
using rharel.M3PD.Common.Hashing;

namespace rharel.M3PD.Agency.System
{
    /// <summary>
    /// Represents a snapshot of an agency system's activity.
    /// </summary>
    public struct SystemActivity
    {
        /// <summary>
        /// Creates a new snapshot.
        /// </summary>
        /// <param name="recent_move">
        /// The latest dialogue move realized by the system.
        /// </param>
        /// <param name="target_move">
        /// The system's target dialogue move.
        /// </param>
        /// <param name="is_active">
        /// True iff the system is currently realizing any move but the idle
        /// move.
        /// </param>
        internal SystemActivity(Optional<DialogueMove> recent_move,
                                DialogueMove target_move,
                                bool is_active)
        {
            Require.IsNotNull(target_move);

            RecentMove = recent_move;
            TargetMove = target_move;
            IsActive = is_active;
        }

        /// <summary>
        /// Gets the dialogue move which has completed realization by the 
        /// system in the previous iteration of the perception/action cycle.
        /// </summary>
        /// <remarks>
        /// This refers to complete moves only. If during the last iteration
        /// a move was still in progress this is assigned 
        /// <see cref="None{DialogueMove}"/>.
        /// </remarks>
        public Optional<DialogueMove> RecentMove { get; private set; }
        /// <summary>
        /// Gets the system's target dialogue move.
        /// </summary>
        public DialogueMove TargetMove { get; private set; }

        /// <summary>
        /// Indicates whether the system is realizing any move but the idle
        /// move.
        /// </summary>
        public bool IsActive { get; private set; }
        /// <summary>
        /// Indicates whether the system is idle.
        /// </summary>
        public bool IsPassive => !IsActive;

        /// <summary>
        /// Determines whether the specified object is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>
        /// True iff the specified object is equal to this instance.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is SystemActivity other)
            {
                return other.RecentMove.Equals(RecentMove) &&
                       other.TargetMove.Equals(TargetMove) &&
                       other.IsActive.Equals(IsActive);
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
            hash = HashCombiner.Hash(hash, RecentMove);
            hash = HashCombiner.Hash(hash, TargetMove);
            hash = HashCombiner.Hash(hash, IsActive);

            return hash;
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(SystemActivity)}{{ " +
                   $"{nameof(RecentMove)} = {RecentMove}, " +
                   $"{nameof(TargetMove)} = {TargetMove}, " +
                   $"{nameof(IsActive)} = {IsActive} }}";
        }
    }
}
