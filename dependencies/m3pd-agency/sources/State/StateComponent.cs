using rharel.Debug;
using rharel.M3PD.Common.Hashing;
using System;

namespace rharel.M3PD.Agency.State
{
    /// <summary>
    /// Represents a container for data.
    /// </summary>
    /// <remarks>
    /// <see cref="StateComponent{T}"/> for the generic version.
    /// </remarks>
    internal interface StateComponent
    {
        /// <summary>
        /// Gets this component's value type.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Gets the contained value.
        /// </summary>
        object Value { get; }
    }
    /// <summary>
    /// Represents a container for data.
    /// </summary>
    /// <typeparam name="T">The type of value the component holds.</typeparam>
    /// <remarks>
    /// <see cref="StateComponent"/> for the non-generic version.
    /// </remarks>
    internal struct StateComponent<T>: StateComponent
    {
        /// <summary>
        /// Creates a new component holding the specified data.
        /// </summary>
        /// <param name="value">The data to hold.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="value"/> is null.
        /// </exception>
        public StateComponent(T value)
        {
            Require.IsNotNull(value);

            Value = value;
        }

        /// <summary>
        /// Gets the type of data being held.
        /// </summary>
        public Type Type => typeof(T);

        /// <summary>
        /// Gets the data being held.
        /// </summary>
        public T Value { get; private set; }
        /// <summary>
        /// Gets the data being held.
        /// </summary>
        object StateComponent.Value => Value;

        /// <summary>
        /// Determines whether the specified object is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>
        /// True iff the specified object is equal to this instance. 
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is StateComponent other)
            {
                if (other.Value is T other_value)
                {
                    return other_value.Equals(Value);
                }
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
            hash = HashCombiner.Hash(hash, Value);

            return hash;
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(StateComponent<T>)}{{ " +
                   $"{nameof(Type)} = {Type}, " +
                   $"{nameof(Value)} = {Value} }}";
        }
    }
}
