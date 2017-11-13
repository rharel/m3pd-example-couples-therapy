using Generator = System.Random;

namespace rharel.M3PD.CouplesTherapyExample.Random
{
    /// <summary>
    /// Generates pseudo-random numbers from a uniform distribution.
    /// </summary>
    internal static class Uniform
    {
        /// <summary>
        /// Obtains a uniform sample from the specified inclusive interval.
        /// </summary>
        /// <param name="min">The lower bound.</param>
        /// <param name="max">The upper bound.</param>
        /// <returns>
        /// A sample between <paramref name="min"/> and 
        /// <paramref name="max"/> (inclusive).
        /// </returns>
        public static float Sample(float min, float max)
        {
            return min + (float)_generator.NextDouble() * (max - min);
        }
        private static readonly Generator _generator = new Generator();
    }
}
