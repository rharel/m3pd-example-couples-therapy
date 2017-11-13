using rharel.Functional;
using rharel.M3PD.Agency.Modules;
using rharel.M3PD.Common.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using static rharel.Functional.Option;

namespace rharel.M3PD.Expectations.Arrangement
{
    /// <summary>
    /// This class represents a node of a social practice program.
    /// </summary>
    public abstract class Node
    {
        /// <summary>
        /// Occurs when the node's resolution status transitions out of 
        /// <see cref="Resolution.Pending"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="Resolved"/> is invoked before <see cref="Failed"/> and
        /// <see cref="Satisfied"/>.
        /// </remarks>
        public event Common.Delegates.EventHandler<Node> Resolved = (
            delegate { }
        );
        /// <summary>
        /// Occurs when the node is resolved with 
        /// <see cref="Resolution.Failure"/>.
        /// </summary>
        public event Common.Delegates.EventHandler<Node> Failed = delegate { };
        /// <summary>
        /// Occurs when the node is resolved with
        /// <see cref="Resolution.Satisfaction"/>.
        /// </summary>
        public event Common.Delegates.EventHandler<Node> Satisfied = (
            delegate { }
        );

        /// <summary>
        /// Creates a new node with the specified identifier and children.
        /// </summary>
        /// <param name="id">The node's identifier.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="id"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="id"/> is blank.
        /// </exception>
        protected Node(string id): this(id, new Node[0]) { }
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
        /// </exception>
        protected Node(string id, IEnumerable<Node> children)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (id.Trim().Length == 0)
            {
                throw new ArgumentException(nameof(id));
            }
            if (children == null)
            {
                throw new ArgumentNullException(nameof(children));
            }
            ID = id;
            Children = new ListView<Node>(children.ToList());
        }

        /// <summary>
        /// Gets this node's title.
        /// </summary>
        public string ID { get; }
        /// <summary>
        /// Gets this node's children.
        /// </summary>
        public ImmutableList<Node> Children { get; }

        /// <summary>
        /// Gets the index of this node's scope carrier child (if it has any).
        /// </summary>
        public virtual Optional<int> ScopeCarrierIndex
            { get; protected set; } = None<int>();
        
        /// <summary>
        /// Gets this node's resolution status.
        /// </summary>
        public Resolution Resolution { get; private set; } = (
            Resolution.Pending
        );

        /// <summary>
        /// Indicates whether this node's expectations have been resolved in 
        /// one way or another.
        /// </summary>
        public bool IsResolved => Resolution != Resolution.Pending;
        /// <summary>
        /// Indicates whether <see cref="Resolution"/> is
        /// <see cref="Resolution.Pending"/>
        /// </summary>
        public bool IsPending => Resolution == Resolution.Pending;
        /// <summary>
        /// Indicates whether <see cref="Resolution"/> is
        /// <see cref="Resolution.Failure"/>
        /// </summary>
        public bool IsFailed => Resolution == Resolution.Failure;
        /// <summary>
        /// Indicates whether <see cref="Resolution"/> is
        /// <see cref="Resolution.Satisfaction"/>
        /// </summary>
        public bool IsSatisfied => Resolution == Resolution.Satisfaction;

        /// <summary>
        /// Resets this node's expectations.
        /// </summary>
        /// <param name="do_recurse">
        /// Indicates whether this node's children should be recursively reset 
        /// as well.
        /// </param>
        /// <remarks>
        /// During a recursive reset, children are reset before the parent.
        /// </remarks>
        public void Reset(bool do_recurse = true)
        {
            Resolution = Resolution.Pending;

            if (do_recurse)
            {
                foreach (var child in Children) { child.Reset(); }
            }

            OnReset();
        }
        /// <summary>
        /// Called by <see cref="Reset"/> to reset this node's expectations
        /// back to their initial state.
        /// </summary>
        protected virtual void OnReset() { }

        /// <summary>
        /// Sets this node's resolution status to 
        /// <see cref="Resolution.Failure"/> and invokes related events.
        /// </summary>
        public void Fail()
        {
            Resolution = Resolution.Failure;
            Resolved.Invoke(this);
            Failed.Invoke(this);
        }
        /// <summary>
        /// Sets this node's resolution status to
        /// <see cref="Resolution.Satisfaction"/> and invokes related events.
        /// </summary>
        public void Satisfy()
        {
            Resolution = Resolution.Satisfaction;
            Resolved.Invoke(this);
            Satisfied.Invoke(this);
        }

        /// <summary>
        /// Processes the specified dialogue event and updates this node's 
        /// expectations accordingly.
        /// </summary>
        /// <param name="event">The event to process.</param>
        /// <returns>The value of <see cref="Resolution"/>.</returns>
        /// <remarks>
        /// Has no effect when this node <see cref="IsResolved"/>.
        /// </remarks>
        public Resolution Process(DialogueEvent @event)
        {
            if (IsResolved) { return Resolution; }
            else
            {
                switch (OnProcess(@event))
                {
                    case Resolution.Failure: Fail(); break;
                    case Resolution.Satisfaction: Satisfy(); break;
                }
                return Resolution;
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
        protected abstract Resolution OnProcess(DialogueEvent @event);

        /// <summary>
        /// Computes and adds the expected event set to the specified 
        /// collection.
        /// </summary>
        /// <param name="result">The collection to add events to.</param>
        public abstract void GetExpectedEvents(
            ICollection<DialogueEvent> result
        );
        /// <summary>
        /// Computes and adds this node's scope carrier chain to the specified
        /// collection (in order).
        /// </summary>
        /// <param name="result">The collection to add nodes to.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="result"/> is null.
        /// </exception>
        public void GetScopeCarrierChain(ICollection<Node> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            result.Add(this);
            ScopeCarrierIndex.ForSome(index =>
            {
                Children[index].GetScopeCarrierChain(result);
            });
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(Node)}{{ " +
                   $"{nameof(ID)} = '{ID}', " +
                   $"{nameof(Resolution)} = {Resolution} }}";
        }
    }
}
