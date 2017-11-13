using rharel.Functional;
using rharel.M3PD.Agency.Modules;
using System;
using System.Collections.Generic;
using static rharel.Functional.Option;

namespace rharel.M3PD.Expectations.Arrangement
{
    /// <summary>
    /// Expects to satisfy exactly one child. The child selected to be the one 
    /// satisfied is the first whose scope carrier chain's endpoint is 
    /// partially satisfied.
    /// </summary>
    public sealed class Divergence: Node
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
        public Divergence(string id, IEnumerable<Node> children)
            : base(id, children)
        {
            if (Children.Count < 2)
            {
                throw new ArgumentException(nameof(children));
            }

            // Retrieve scope carrier chain endpoint for each child:
            var chain = new List<Node>();
            foreach (var child in Children)
            {
                chain.Clear();
                child.GetScopeCarrierChain(chain);
                _scope_carrier_endpoints.Add(chain[chain.Count - 1]);
            }

            _disjunction = new Disjunction($"{id}.disjunction", Children);
        }

        /// <summary>
        /// Gets the index of the selected child (if it has been already 
        /// determined).
        /// </summary>
        public Optional<int> SelectedChildIndex { get; private set; } = (
            None<int>()
        );
        /// <summary>
        /// Gets the index of this node's active scope child (if it has any).
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
                    return SelectedChildIndex;
                }
            }
        }

        /// <summary>
        /// Called by <see cref="Node.Reset"/> to reset this node's 
        /// expectations back to their initial state.
        /// </summary>
        protected override void OnReset()
        {
            _disjunction.Reset(do_recurse: false);
            SelectedChildIndex = None<int>();
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
            return SelectedChild.MapSomeOrElse
            (
                node => { return node.Process(@event); },
                () =>
                {
                    _disjunction.Process(@event);
                    if (_disjunction.IsResolved)
                    {
                        return _disjunction.Resolution;
                    }

                    int index = _scope_carrier_endpoints.FindIndex(
                        node => node.IsSatisfied
                    );
                    if (index != -1)
                    {
                        SelectedChildIndex = Some(index);
                    }
                    return Resolution.Pending;
                }
            );
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

            SelectedChild.ForSomeOrElse(
                node => node.GetExpectedEvents(result),
                () => _disjunction.GetExpectedEvents(result)
            );
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(Divergence)}(" +
                   $"{nameof(ID)} = '{ID}', " +
                   $"{nameof(SelectedChild)} = {SelectedChild}, " +
                   $"{nameof(Children)} = {Children})";
        }

        /// <summary>
        /// Gets the selected child to be satisfied (if it has already been 
        /// determined).
        /// </summary>
        private Optional<Node> SelectedChild => 
            SelectedChildIndex.MapSomeOr(
                index => Some(Children[index]),
                None<Node>()
            );

        private readonly List<Node> _scope_carrier_endpoints = (
            new List<Node>()
        );
        private readonly Disjunction _disjunction;
    }
}
