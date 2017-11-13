using rharel.Debug;

namespace rharel.M3PD.CouplesTherapyExample.Time
{
    /// <summary>
    /// Represents a <see cref="Clock"/> with a variable-sized increment.
    /// </summary>
    internal sealed class VariableIncrementClock: Clock
    {
        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <remarks>
        /// The initial time value is zero.
        /// </remarks>
        public float Time { get; private set; } = 0.0f;
        /// <summary>
        /// Gets the latest time increment's size.
        /// </summary>
        /// <remarks>
        /// The initial increment value is zero.
        /// </remarks>
        public float TimeIncrement { get; private set; } = 0.0f;

        /// <summary>
        /// Advances time by the specified increment.
        /// </summary>
        /// <param name="increment">The time increment.</param>
        public void Tick(float increment = 1.0f)
        {
            Require.IsGreaterThan(increment, 0);

            Time += increment;
            TimeIncrement = increment;
        }

        /// <summary>
        /// Determines whether the specified object is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>
        /// True if the specified object is equal to this instance; 
        /// otherwise false.
        /// </returns>
        public override bool Equals(object obj)
        {
            var other = obj as VariableIncrementClock;

            if (ReferenceEquals(other, null)) { return false; }
            if (ReferenceEquals(other, this)) { return true; }

            return other.Time.Equals(Time);
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
            return Time.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(VariableIncrementClock)}{{ " +
                   $"{nameof(Time)} = {Time}, " +
                   $"{nameof(TimeIncrement)} = {TimeIncrement} }}";
        }
    }
}
