using rharel.M3PD.Agency.Dialogue_Moves;
using rharel.M3PD.Common.DesignPatterns;

namespace rharel.M3PD.Expectations.Arrangement
{
    /// <summary>
    /// Helps make move composition easier via an interface that is closer to 
    /// natural language descriptions of dialogue moves.
    /// </summary>
    /// <example>
    /// <code>
    /// using static rharel.Expectations.Arrangement.DialogueMoveNLI;
    /// 
    /// struct Greeting { ... }
    /// ...
    /// Move("greeting", 
    ///      addressee: "Alice",
    ///      properties: new Greeting(...)
    /// );
    /// </code>
    /// </example>
    /// <remarks>
    /// NLI stands for Natural Language Interface.
    /// </remarks>
    public static class DialogueMoveNLI
    {
        /// <summary>
        /// Builds instances of <see cref="DialogueMove{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public sealed class Builder<T>: ObjectBuilder<DialogueMove<T>>
        {
            /// <summary>
            /// Builds implicitly.
            /// </summary>
            /// <param name="builder">The instance to build.</param>
            public static implicit operator DialogueMove<T>(Builder<T> builder)
            {
                return builder.Build();
            }

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
                _builder = new DialogueMove<T>.Builder(type);
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
            public Builder<T> WithAddressees(params string[] ids)
            {
                _builder.WithAddressees(ids);

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
            public Builder<T> WithProperties(T value)
            {
                _builder.WithProperties(value);

                return this;
            }

            /// <summary>
            /// Creates the object.
            /// </summary>
            /// <returns>The built object.</returns>
            protected override DialogueMove<T> CreateObject()
            {
                return _builder.Build();
            }

            private readonly DialogueMove<T>.Builder _builder;
        }

        /// <summary>
        /// Creates a new non-generic builder for a move of the specified type.
        /// </summary>
        /// <param name="type">The move's type identifier.</param>
        /// <returns>A new move.</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="type"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="type"/> is blank.
        /// </exception>
        public static Builder<object> Move(string type)
        {
            return new Builder<object>(type);
        }
        /// <summary>
        /// Creates a new non-generic builder for a move of the specified type.
        /// </summary>
        /// <param name="type">The move's type identifier.</param>
        /// <returns>A new move.</returns>
        /// <exception cref="ArgumentException">
        /// When <paramref name="type"/> is blank.
        /// </exception>
        public static Builder<T> Move<T>(
            string addressee = null, 
            string type = null) 
            where T: new()
        {
            return Move(type, addressee, new T());
        }
        /// <summary>
        /// Creates a new non-generic builder for a move of the specified type.
        /// </summary>
        /// <param name="type">The move's type identifier.</param>
        /// <returns>A new move.</returns>
        /// <exception cref="ArgumentException">
        /// When <paramref name="type"/> is blank.
        /// </exception>
        public static Builder<T> Move<T>(
            T properties,
            string addressee = null,
            string type = null)
        {
            return Move(type, addressee, properties);
        }
        /// <summary>
        /// Creates a new non-generic builder for a move of the specified type.
        /// </summary>
        /// <param name="type">The move's type identifier.</param>
        /// <returns>A new move.</returns>
        /// <exception cref="ArgumentException">
        /// When <paramref name="type"/> is blank.
        /// </exception>
        private static Builder<T> Move<T>(
            string type,
            string addressee,
            T properties)
        {
            if (type == null) { type = typeof(T).Name; }

            var builder = new Builder<T>(type);

            if (properties != null) { builder.WithProperties(properties); }
            if (addressee != null) { builder.WithAddressees(addressee); }

            return builder;
        }
    }
}
