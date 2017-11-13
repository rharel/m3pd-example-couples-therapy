using rharel.M3PD.Common.Hashing;
using rharel.M3PD.Expectations.Arrangement;
using System;

namespace rharel.M3PD.Expectations.State
{
    /// <summary>
    /// Represents the state components required by 
    /// <see cref="Modules.ExpectationBasedAS"/> and <see cref="Modules.ExpectationBasedAT"/>.
    /// </summary>
    public struct SocialContext
    {
        /// <summary>
        /// Creates a new state for the specified agent and using the specified
        /// expectation arrangement.
        /// </summary>
        /// <param name="self_id">The agent representing the system.</param>
        /// <param name="interaction">
        /// The interaction's expectation arrangement.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="self_id"/> is null.
        /// When <paramref name="interaction"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="self_id"/> is blank.
        /// </exception>
        public SocialContext(string self_id, Node interaction)
        {
            if (self_id == null)
            {
                throw new ArgumentNullException(nameof(self_id));
            }
            if (self_id.Trim().Length == 0)
            {
                throw new ArgumentException(nameof(self_id));
            }
            if (interaction == null)
            {
                throw new ArgumentNullException(nameof(interaction));
            }
            SelfID = self_id;
            Interaction = interaction;
        }

        /// <summary>
        /// Gets the identifier of the agent representing the system.
        /// </summary>
        public string SelfID { get; }
        /// <summary>
        /// Gets the interaction's expectation arrangement.
        /// </summary>
        public Node Interaction { get; }

        /// <summary>
        /// Determines whether the specified object is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>
        /// True iff the specified object is equal to this instance. 
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is SocialContext other)
            {
                return other.SelfID.Equals(SelfID) &&
                       other.Interaction.Equals(Interaction);
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
            hash = HashCombiner.Hash(hash, SelfID);
            hash = HashCombiner.Hash(hash, Interaction);

            return hash;
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(SocialContext)}{{ " +
                   $"{nameof(SelfID)} = {SelfID}, " +
                   $"{nameof(Interaction)} = {Interaction} }}";
        }
    }
}
