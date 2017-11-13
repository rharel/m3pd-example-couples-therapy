using rharel.M3PD.Common.Collections;
using System;
using System.Collections.Generic;

namespace rharel.M3PD.Agency.Modules
{
    /// <summary>
    /// The medium through which <see cref="CAPModule"/> reports its output.
    /// </summary>
    public sealed class CurrentActivity
    {
        /// <summary>
        /// Creates a new report.
        /// </summary>
        internal CurrentActivity()
        {
            PassiveIDs = new CollectionView<string>(_passive_ids);
            ActiveIDs = new CollectionView<string>(_active_ids);
        }

        /// <summary>
        /// Gets the collection of passive agents.
        /// </summary>
        public ImmutableCollection<string> PassiveIDs { get; private set; }
        /// <summary>
        /// Gets the collection of active agents.
        /// </summary>
        public ImmutableCollection<string> ActiveIDs { get; private set; }

        /// <summary>
        /// Determines whether the specified identifier is present in this 
        /// report.
        /// </summary>
        /// <param name="id">The identifier to query.</param>
        /// <returns>
        /// True iff <paramref name="id"/> is present in this report.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="id"/> is null.
        /// </exception>
        public bool Contains(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            return PassiveIDs.Contains(id) || 
                   ActiveIDs.Contains(id);
        }
        /// <summary>
        /// Gets the status of the agent with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier to query.</param>
        /// <returns>
        /// <see cref="ActivityStatus.Passive"/> if the specified agent is 
        /// present in <see cref="PassiveIDs"/>, otherwise
        /// <see cref="ActivityStatus.Active"/>.  
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="id"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="id"/> is not in the report.
        /// </exception>
        public ActivityStatus GetStatus(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            
            if (PassiveIDs.Contains(id))
            {
                return ActivityStatus.Passive;
            }
            else if (ActiveIDs.Contains(id))
            {
                return ActivityStatus.Active;
            }
            else
            {
                throw new ArgumentException(nameof(id));
            }
        }

        /// <summary>
        /// Adds the specified agent to this report, marked as passive.
        /// </summary>
        /// <param name="id">The agent's identifier.</param>
        /// <returns>True iff the agent was added successfully.</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="id"/> is null.
        /// </exception>
        /// <remarks>
        /// An addition is unsuccessful when the agent is already present in 
        /// this report.
        /// </remarks>
        public bool MarkPassive(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (Contains(id)) { return false; }
            else
            {
                _passive_ids.Add(id);
                return true;
            }
        }
        /// <summary>
        /// Adds the specified agent to this report, marked as passive.
        /// </summary>
        /// <param name="id">The agent's identifier.</param>
        /// <returns>True iff the agent was added successfully.</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="id"/> is null.
        /// </exception>
        /// <remarks>
        /// An addition is unsuccessful when the agent is already present in 
        /// this report.
        /// </remarks>
        public bool MarkActive(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (Contains(id)) { return false; }
            else
            {
                _active_ids.Add(id);
                return true;
            }
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(CurrentActivity)}{{ " +
                   $"{nameof(PassiveIDs)} = {PassiveIDs}, " + 
                   $"{nameof(ActiveIDs)} = {ActiveIDs} }}";
        }

        private readonly ICollection<string> _active_ids = (
            new HashSet<string>()
        );
        private readonly ICollection<string> _passive_ids = (
            new HashSet<string>()
        );
    }
}
