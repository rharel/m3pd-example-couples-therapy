using rharel.Functional;
using rharel.M3PD.Common.Collections;
using rharel.M3PD.Common.DesignPatterns;
using rharel.M3PD.Common.Hashing;
using System;
using System.Collections.Generic;
using System.Linq;
using static rharel.Functional.Option;

namespace rharel.M3PD.Agency.Dialogue_Moves
{
    /// <summary>
    /// Represents a communicated meaning.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Dialogue moves are a way to refer to the act of communicating meanings 
    /// through behavior, independent of the concrete form(s) such behavior may 
    /// take.
    /// </para>
    /// <para>
    /// To illustrate: both a hand-wave as well as a verbal "hello" may be
    /// regarded as concrete forms of the same move: a greeting. The level of
    /// abstraction desired is dependent on the use-case.
    /// </para>
    /// <para>
    /// <see cref="Move{T}"/> for the generic version.
    /// </para>
    /// </remarks>
    public interface DialogueMove
    {
        /// <summary>
        /// Gets this move's type identifier.
        /// </summary>
        string Type { get; }
        /// <summary>
        /// Gets this move's addressee identifiers.
        /// </summary>
        ImmutableCollection<string> AddresseeIDs { get; }
        /// <summary>
        /// Gets this move's properties.
        /// </summary>
        Optional<object> Properties { get; }
    }
    /// <summary>
    /// Represents a communicated meaning.
    /// </summary>
    /// <typeparam name="T">The type of the move's properties.</typeparam>
    /// <remarks>
    /// <see cref="DialogueMove"/> for the non-generic version and additional
    /// details.
    /// </remarks>
    public sealed class Move<T>: DialogueMove
    {
        /// <summary>
        /// Builds instances of <see cref="Move{T}"/>.
        /// </summary>
        public sealed class Builder: ObjectBuilder<Move<T>>
        {
            /// <summary>
            /// Creates a new builder for a move of the specified type.
            /// </summary>
            /// <param name="type">The move's type identifier.</param>
            /// <exception cref="ArgumentNullException">
            /// When <paramref name="type"/> is null.
            /// </exception>
            /// <exception cref="ArgumentException">
            /// When <paramref name="type"/> is blank.
            /// </exception>
            public Builder(string type)
            {
                if (type == null)
                {
                    throw new ArgumentNullException(nameof(type));
                }
                if (type.Trim().Length == 0)
                {
                    throw new ArgumentException(nameof(type));
                }
                _type = type;
            }

            /// <summary>
            /// Adds the specified identifier as an addressee.
            /// </summary>
            /// <param name="id">The addressee's identifier.</param>
            /// <returns>This instance (for method call-chaining).</returns>
            /// <exception cref="InvalidOperationException">
            /// When called on a built object.
            /// </exception>
            /// <exception cref="ArgumentNullException">
            /// When <paramref name="id"/> is null.
            /// </exception>
            /// <exception cref="ArgumentException">
            /// When <paramref name="id"/> is blank.
            /// </exception>
            public Builder WithAddressee(string id)
            {
                if (IsBuilt)
                {
                    throw new InvalidOperationException(
                        "Object is already built."
                    );
                }
                if (id == null)
                {
                    throw new ArgumentNullException(nameof(id));
                }
                if (id.Trim().Length == 0)
                {
                    throw new ArgumentException(nameof(id));
                }

                _addressee_ids.Add(id);

                return this;
            }
            /// <summary>
            /// Adds the specified identifiers as an addressees.
            /// </summary>
            /// <param name="ids">The addressees' identifiers.</param>
            /// <returns>This instance (for method call-chaining).</returns>
            /// <exception cref="InvalidOperationException">
            /// When called on a built object.
            /// </exception>
            /// <exception cref="ArgumentNullException">
            /// When <paramref name="ids"/> is null.
            /// When <paramref name="ids"/> contains null.
            /// </exception>
            /// <exception cref="ArgumentException">
            /// When <paramref name="ids"/> contains a blank identifier.
            /// </exception>
            public Builder WithAddressees(IEnumerable<string> ids)
            {
                if (IsBuilt)
                {
                    throw new InvalidOperationException(
                        "Object is already built."
                    );
                }
                if (ids == null)
                {
                    throw new ArgumentNullException(nameof(ids));
                }

                _addressee_ids.UnionWith(ids);

                return this;
            }
            /// <summary>
            /// Adds the specified properties.
            /// </summary>
            /// <param name="value">The value to assign.</param>
            /// <returns>This instance (for method call-chaining).</returns>
            /// <exception cref="InvalidOperationException">
            /// When called on a built object.
            /// </exception>
            public Builder WithProperties(T value)
            {
                if (IsBuilt)
                {
                    throw new InvalidOperationException(
                        "Object is already built."
                    );
                }

                _properties = Some(value);

                return this;
            }

            /// <summary>
            /// Creates the object.
            /// </summary>
            /// <returns>The built object.</returns>
            protected override Move<T> CreateObject()
            {
                return new Move<T>(_type, _addressee_ids, _properties);
            }

            private readonly string _type;
            private HashSet<string> _addressee_ids = new HashSet<string>();
            private Optional<T> _properties = None<T>();
        }
        /// <summary>
        /// Creates a new move of the specified type and properties.
        /// </summary>
        /// <param name="type">The move's type identifier.</param>
        /// <param name="addressee_ids">
        /// The move's addressee identifiers.
        /// </param>
        /// <param name="properties">The move's properties.</param>
        private Move(
            string type, 
            HashSet<string> addressee_ids, 
            Optional<T> properties)
        {
            Type = type;
            AddresseeIDs = new CollectionView<string>(addressee_ids);
            Properties = properties;
        }

        /// <summary>
        /// Gets this move's type identifier.
        /// </summary>
        public string Type { get; }
        /// <summary>
        /// Gets this move's addressee identifiers.
        /// </summary>
        public ImmutableCollection<string> AddresseeIDs { get; }

        /// <summary>
        /// Gets this move's properties.
        /// </summary>
        /// <remarks>
        /// <see cref="DialogueMove.Properties"/> for the non-generic version.
        /// </remarks>
        public Optional<T> Properties { get; }
        /// <summary>
        /// Gets this move's properties.
        /// </summary>
        /// <remarks>
        /// <see cref="Properties"/> for the generic version.
        /// </remarks>
        Optional<object> DialogueMove.Properties => Properties.Cast<object>();

        /// <summary>
        /// Casts the properties of this and wraps it in a new move of the same
        /// type.
        /// </summary>
        /// <typeparam name="TResult">The type to cast to.</typeparam>
        /// <returns>
        /// A new move containing the cast properties of this one.
        /// </returns>
        public Move<TResult> Cast<TResult>()
        {
            return new Move<TResult>(
                Type, 
                new HashSet<string>(AddresseeIDs), 
                Properties.Cast<TResult>()
            );
        }

        /// <summary>
        /// Determines whether the specified object is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>
        /// True iff the specified object is equal to this instance. 
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is DialogueMove other)
            {
                return other.Type.Equals(Type) &&
                       other.AddresseeIDs.All(
                           id => AddresseeIDs.Contains(id)
                       ) &&
                       other.Properties.Equals(Properties);
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
            hash = HashCombiner.Hash(hash, Type);
            hash = HashCombiner.Hash(hash, AddresseeIDs);
            hash = HashCombiner.Hash(hash, Properties);
            
            return hash;
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            string addressees = String.Join(", ", AddresseeIDs.ToArray());

            return $"{nameof(Move<T>)}{{ " +
                   $"{nameof(Type)} = '{Type}', " +
                   $"{nameof(AddresseeIDs)} = [{addressees}], " +
                   $"{nameof(Properties)} = {Properties} }}";
        }
    }
    /// <summary>
    /// Convenience methods for <see cref="DialogueMove"/> instantiation.
    /// </summary>
    public static class DialogueMoves
    {
        /// <summary>
        /// Creates a new move of the specified type and optional addressee.
        /// </summary>
        /// <param name="type">The move's type identifier.</param>
        /// <param name="addressee">The move's addressee (optional).</param>
        /// <returns>A new move.</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="type"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="type"/> is blank.
        /// When <paramref name="addressee"/> is blank.
        /// </exception>
        public static DialogueMove Create(string type, string addressee = null)
        {
            var move = new Move<object>.Builder(type);

            if (addressee != null) { move.WithAddressee(addressee); }

            return move.Build();
        }
        /// <summary>
        /// Creates a new move of the specified type and addressees.
        /// </summary>
        /// <param name="type">The move's type identifier.</param>
        /// <param name="addressees">The move's addressees.</param>
        /// <returns>A new move.</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="type"/> is null.
        /// When <paramref name="addressees"/> is null or contains null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="type"/> is blank.
        /// When <paramref name="addressees"/> contains blank.
        /// </exception>
        public static DialogueMove Create(
            string type, IEnumerable<string> addressees)
        {
            var move = new Move<object>.Builder(type);

            if (addressees != null) { move.WithAddressees(addressees); }

            return move.Build();
        }
        /// <summary>
        /// Creates a new move of the specified type and addressees.
        /// </summary>
        /// <param name="type">The move's type identifier.</param>
        /// <param name="addressees">The move's addressees.</param>
        /// <returns>A new move.</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="type"/> is null.
        /// When <paramref name="addressees"/> is null or contains null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="type"/> is blank.
        /// When <paramref name="addressees"/> contains blank.
        /// </exception>
        public static DialogueMove Create(
            string type, params string[] addressees)
        {
            var move = new Move<object>.Builder(type);

            if (addressees != null) { move.WithAddressees(addressees); }

            return move.Build();
        }

        /// <summary>
        /// Creates a new move of the specified type, properties and optional 
        /// addressee.
        /// </summary>
        /// <param name="type">The move's type identifier.</param>
        /// <param name="properties">The move's properties.</param>
        /// <param name="addressee">The move's addressee (optional).</param>
        /// <returns>A new move.</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="type"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="type"/> is blank.
        /// When <paramref name="addressee"/> is blank.
        /// </exception>
        public static Move<T> Create<T>(
            string type, T properties, string addressee = null)
        {
            var move = new Move<T>.Builder(type);
            move.WithProperties(properties);

            if (addressee != null) { move.WithAddressee(addressee); }

            return move.Build();
        }
        /// <summary>
        /// Creates a new move of the specified type, properties, and 
        /// addressees.
        /// </summary>
        /// <param name="type">The move's type identifier.</param>
        /// <param name="properties">The move's properties.</param>
        /// <param name="addressees">The move's addressees.</param>
        /// <returns>A new move.</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="type"/> is null.
        /// When <paramref name="addressees"/> is null or contains null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="type"/> is blank.
        /// When <paramref name="addressees"/> contains blank.
        /// </exception>
        public static Move<T> Create<T>(
            string type, T properties, IEnumerable<string> addressees)
        {
            var move = new Move<T>.Builder(type);
            move.WithProperties(properties);

            if (addressees != null) { move.WithAddressees(addressees); }

            return move.Build();
        }
        /// <summary>
        /// Creates a new move of the specified type, properties, and 
        /// addressees.
        /// </summary>
        /// <param name="type">The move's type identifier.</param>
        /// <param name="properties">The move's properties.</param>
        /// <param name="addressees">The move's addressees.</param>
        /// <returns>A new move.</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="type"/> is null.
        /// When <paramref name="addressees"/> is null or contains null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="type"/> is blank.
        /// When <paramref name="addressees"/> contains blank.
        /// </exception>
        public static Move<T> Create<T>(
            string type, T properties, params string[] addressees)
        {
            var move = new Move<T>.Builder(type);
            move.WithProperties(properties);

            if (addressees != null) { move.WithAddressees(addressees); }

            return move.Build();
        }
    }
}
