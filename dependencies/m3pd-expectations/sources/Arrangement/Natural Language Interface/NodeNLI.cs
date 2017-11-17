using rharel.Debug;
using rharel.M3PD.Agency.Dialogue_Moves;
using rharel.M3PD.Agency.Modules;
using rharel.M3PD.Common.Collections;
using rharel.M3PD.Common.Delegates;
using System;
using System.Collections.Generic;

namespace rharel.M3PD.Expectations.Arrangement
{
    /// <summary>
    /// Helps make program composition easier via an interface that is closer to 
    /// natural language descriptions of expectation arrangements.
    /// </summary>
    /// <example>
    /// <code>
    /// var expect = new NodeNLI();
    /// expect.Sequence("some sequence",
    ///     expect.Event(...),
    ///     expect.Conjunction(...),
    ///     ...
    /// );
    /// </code>
    /// </example>
    /// <remarks>
    /// <para>
    /// If during the creation of a new node no identifier is supplied, one
    /// is generated automatically. Identifiers prefixed with an _underscore
    /// should be considered reserved for this purpose.
    /// </para>
    /// <para>
    /// NLI stands for Natural Language Interface.
    /// </para>
    /// </remarks>
    public sealed class NodeNLI
    {
        /// <summary>
        /// Creates a new program composition interface.
        /// </summary>
        public NodeNLI()
        {
            NodeIDs = new CollectionView<string>(_node_ids);
        }

        /// <summary>
        /// Gets node identifiers for nodes created via this instance.
        /// </summary>
        public ImmutableCollection<string> NodeIDs { get; }

        /// <summary>
        /// Expects a specified event indefinitely.
        /// </summary>
        /// <param name="event">The expected event.</param>
        /// <returns>A new expectation node.</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="event"/> is null.
        /// </exception>
        public IndefiniteEvent IndefiniteEvent(DialogueEvent @event)
        {
            string id = ProcessNewNode(typeof(IndefiniteEvent), null);
            return new IndefiniteEvent(id, @event);
        }
        /// <summary>
        /// Expects a specified event indefinitely.
        /// </summary>
        /// <param name="source">
        /// The identifier of the agent who realized the move.
        /// </param>
        /// <param name="move">The move that was realized.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="source"/> is null.
        /// When <paramref name="move"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="source"/> is blank.
        /// </exception>
        public IndefiniteEvent IndefiniteEvent(
            string source, DialogueMove move)
        {
            var @event = new DialogueEvent(source, move);
            return IndefiniteEvent(@event);
        }
        /// <summary>
        /// Expects a specified event indefinitely.
        /// </summary>
        /// <param name="source">
        /// The identifier of the agent who realized the move.
        /// </param>
        /// <param name="move">The move that was realized.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="source"/> is null.
        /// When <paramref name="move"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="source"/> is blank.
        /// </exception>
        public IndefiniteEvent IndefiniteEvent<T>(
            string source, DialogueMoveNLI.Builder<T> move)
        {
            return IndefiniteEvent(source, move.Build());
        }

        /// <summary>
        /// Expects to satisfy a specified child only while a specified 
        /// condition holds.
        /// </summary>
        /// <param name="condition">The condition to apply.</param>
        /// <param name="body">The node's only child.</param>
        /// <returns>A new expectation node.</returns>
        /// <exception cref="ArgumentNullException">
        /// When either <paramref name="body"/> or 
        /// <paramref name="condition"/> is null.
        /// </exception>
        public Conditional Conditional(Predicate condition, Node body)
        {
            string id = ProcessNewNode(typeof(Conditional), null);
            return new Conditional(id, body, condition);
        }

        /// <summary>
        /// Expects satisfaction of all children in no particular order.
        /// </summary>
        /// <param name="id">The node's identifier.</param>
        /// <param name="children">The node's children.</param>
        /// <returns>A new expectation node.</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="children"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="id"/> is blank or already taken.
        /// When <paramref name="children"/> contains less than two members.
        /// </exception>
        public Conjunction Conjunction(
            string id, params Node[] children)
        {
            id = ProcessNewNode(typeof(Conjunction), id);
            return new Conjunction(id, children);
        }

        /// <summary>
        /// Expects satisfaction of at least one child.
        /// </summary>
        /// <param name="id">The node's identifier.</param>
        /// <param name="children">The node's children.</param>
        /// <returns>A new expectation node.</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="children"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="id"/> is blank or already taken.
        /// When <paramref name="children"/> contains less than two members.
        /// </exception>
        public Disjunction Disjunction(
            string id, params Node[] children)
        {
            id = ProcessNewNode(typeof(Disjunction), id);
            return new Disjunction(id, children);
        }

        /// <summary>
        /// Expects to satisfy exactly one child. The child selected to be the 
        /// one satisfied is the first whose scope carrier chain's endpoint is 
        /// partially satisfied.
        /// </summary>
        /// <param name="id">The node's identifier.</param>
        /// <param name="children">The node's children.</param>
        /// <returns>A new expectation node.</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="children"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="id"/> is blank or already taken.
        /// When <paramref name="children"/> contains less than two members.
        /// </exception>
        public Divergence Divergence(string id, params Node[] children)
        {
            id = ProcessNewNode(typeof(Divergence), id);
            return new Divergence(id, children);
        }

        /// <summary>
        /// Expects to repeatedly satisfy a specified child.
        /// </summary>
        /// <param name="body">The node's only child.</param>
        /// <returns>A new expectation node.</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="body"/> is null.
        /// </exception>
        public Repetition Repetition(Node body)
        {
            string id = ProcessNewNode(typeof(Repetition), null);
            return new Repetition(id, body);
        }

        /// <summary>
        /// Expects satisfaction of all children in sequence.
        /// </summary>
        /// <param name="id">The node's identifier.</param>
        /// <param name="children">The node's children.</param>
        /// <returns>A new expectation node.</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="children"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="id"/> is blank or already taken.
        /// When <paramref name="children"/> contains less than two members.
        /// </exception>
        public Sequence Sequence(string id, params Node[] children)
        {
            id = ProcessNewNode(typeof(Sequence), id);
            return new Sequence(id, children);
        }

        #region Alias methods

        /// <summary>
        /// Expects satisfaction of all children in no particular order.
        /// </summary>
        /// <param name="id">The node's identifier.</param>
        /// <param name="children">The node's children.</param>
        /// <returns>A conjunctive expectation.</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="children"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="id"/> is blank or already taken.
        /// When <paramref name="children"/> contains less than two members.
        /// </exception>
        /// <remarks>
        /// This is an alias for <see cref="Conjunction(string, Node[])"/>.
        /// </remarks>
        public Conjunction All(string id, params Node[] children)
        {
            return Conjunction(id, children);
        }
        /// <summary>
        /// Expects satisfaction of at least one child.
        /// </summary>
        /// <param name="id">The node's identifier.</param>
        /// <param name="children">The node's children.</param>
        /// <returns>A disjunctive expectation.</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="children"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="id"/> is blank or already taken.
        /// When <paramref name="children"/> contains less than two members.
        /// </exception>
        /// <remarks>
        /// This is an alias for <see cref="Disjunction(string, Node[])"/>.
        /// </remarks>
        public Disjunction Any(string id, params Node[] children)
        {
            return Disjunction(id, children);
        }
        /// <summary>
        /// Expects a specified event indefinitely.
        /// </summary>
        /// <param name="event">The expected event.</param>
        /// <returns>An indefinite event expectation.</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="event"/> is null.
        /// </exception>
        /// <remarks>
        /// This is an alias for 
        /// <see cref="IndefiniteEvent(DialogueEvent)"/>.
        /// </remarks>
        public IndefiniteEvent Event(DialogueEvent @event)
        {
            return IndefiniteEvent(@event);
        }
        /// <summary>
        /// Expects a specified event indefinitely.
        /// </summary>
        /// <param name="source">
        /// The identifier of the agent who realized the move.
        /// </param>
        /// <param name="move">The move that was realized.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="source"/> is null.
        /// When <paramref name="move"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="source"/> is blank.
        /// </exception>
        /// <remarks>
        /// This is an alias for 
        /// <see cref="IndefiniteEvent(string, DialogueMove)"/>.
        /// </remarks>
        public IndefiniteEvent Event(string source, DialogueMove move)
        {
            return IndefiniteEvent(source, move);
        }
        /// <summary>
        /// Expects a specified event indefinitely.
        /// </summary>
        /// <param name="source">
        /// The identifier of the agent who realized the move.
        /// </param>
        /// <param name="move">The move that was realized.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="source"/> is null.
        /// When <paramref name="move"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="source"/> is blank.
        /// </exception>
        /// <remarks>
        /// This is an alias for 
        /// <see cref="IndefiniteEvent(string, DialogueMoveNLI.Builder{T})"/>.
        /// </remarks>
        public IndefiniteEvent Event<T>(
            string source, DialogueMoveNLI.Builder<T> move)
        {
            return IndefiniteEvent(source, move);
        }

        /// <summary>
        /// Expects to satisfy a specified child only while a specified 
        /// condition holds.
        /// </summary>
        /// <param name="condition">The condition to apply.</param>
        /// <param name="body">The node's only child.</param>
        /// <returns>A conditional expectation.</returns>
        /// <exception cref="ArgumentNullException">
        /// When either <paramref name="body"/> or 
        /// <paramref name="condition"/> is null.
        /// </exception>
        /// <remarks>
        /// This is an alias for 
        /// <see cref="Conditional(Node, Predicate)"/>.
        /// </remarks>
        public Conditional If(Predicate condition, Node body)
        {
            return Conditional(condition, body);
        }
        /// <summary>
        /// Expects to satisfy exactly one child. The child selected to be the 
        /// one satisfied is the first whose scope carrier chain's endpoint is 
        /// partially satisfied.
        /// </summary>
        /// <param name="id">The node's identifier.</param>
        /// <param name="children">The node's children.</param>
        /// <returns>A divergent expectation.</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="children"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="id"/> is blank or already taken.
        /// When <paramref name="children"/> contains less than two members.
        /// </exception>
        /// <remarks>
        /// This is an alias for <see cref="Divergence(string, Node[])"/>.
        /// </remarks>
        public Divergence OneOf(string id, params Node[] children)
        {
            return Divergence(id, children);
        }

        #endregion Alias methods
        
        /// <summary>
        /// Validates the specified node identifier.
        /// </summary>
        /// <param name="id">The identifier to process.</param>
        /// <param name="node_type">The node's type.</param>
        /// <returns>
        /// If <paramref name="id"/> is null, a new auto-generated identifier,
        /// otherwise <paramref name="id"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="node_type"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="id"/> is blank or already taken.
        /// When <paramref name="node_type"/> is not recognized.
        /// </exception>
        private string ProcessNewNode(Type node_type, string id)
        {
            if (id == null)
            {
                id = GenerateID(node_type);
            }
            else if (id.Trim().Length == 0 || _node_ids.Contains(id))
            {
                throw new ArgumentException(nameof(id));
            }

            _node_ids.Add(id);
            _node_count_by_type[node_type] += 1;

            return id;
        }
        /// <summary>
        /// Generates an identifier for a node of the specified type.
        /// </summary>
        /// <param name="node_type">The node's type.</param>
        /// <returns>A unique identifier.</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="node_type"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="node_type"/> is not recognized.
        /// </exception>
        private string GenerateID(Type node_type)
        {
            Require.IsNotNull(node_type);
            Require.IsTrue(_node_count_by_type.ContainsKey(node_type));

            return $"_{node_type}_#{_node_count_by_type[node_type]}";
        }

        private readonly HashSet<string> _node_ids = new HashSet<string>();
        private readonly Dictionary<Type, int> _node_count_by_type = (
            new Dictionary<Type, int>
            {
                // Atomics
                { typeof(IndefiniteEvent), 0 },

                // Composites
                { typeof(Conditional), 0 },
                { typeof(Conjunction), 0 },
                { typeof(Disjunction), 0 },
                { typeof(Divergence), 0 },
                { typeof(Repetition), 0 },
                { typeof(Sequence), 0 },
            }
        );
    }
}
