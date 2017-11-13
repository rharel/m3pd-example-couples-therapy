using rharel.Debug;

namespace rharel.M3PD.CouplesTherapyExample.Time
{
    /// <summary>
    /// Represents a countdown timer.
    /// </summary>
    public sealed class Timer
    {
        /// <summary>
        /// Creates a new timer set to the specified duration.
        /// </summary>
        /// <param name="clock">The clock to reference for time.</param>
        /// <param name="duration">The timer's initial duration.</param>
        public Timer(Clock clock, float duration = 0.0f)
        {
            Require.IsNotNull(clock);
            Require.IsAtLeast(duration, 0);

            Clock = clock;
            Set(duration);
        }

        /// <summary>
        /// Gets the clock used to reference time.
        /// </summary>
        public Clock Clock { get; }

        /// <summary>
        /// Gets the total time duration.
        /// </summary>
        public float TotalDuration { get; private set; }
        /// <summary>
        /// Gets the elapsed time duration.
        /// </summary>
        public float ElapsedDuration { get; private set; }
        /// <summary>
        /// Gets the remaining time duration.
        /// </summary>
        public float RemainingDuration { get; private set; }

        /// <summary>
        /// Indicates whether this timer is ticking down.
        /// </summary>
        public bool IsTicking { get; private set; }
        /// <summary>
        /// Indicates whether the timer is ticking down and time's up.
        /// </summary>
        public bool IsRinging { get; private set; }

        /// <summary>
        /// Resets the timer and sets its duration to the specified value.
        /// </summary>
        /// <param name="duration">The desired duration.</param>
        public void Set(float duration)
        {
            Require.IsAtLeast(duration, 0);

            TotalDuration = duration;
            Reset();
        }
        /// <summary>
        /// Resets the timer.
        /// </summary>
        public void Reset()
        {
            ElapsedDuration = 0.0f;
            RemainingDuration = TotalDuration;

            IsTicking = false;
            IsRinging = false;
        }

        /// <summary>
        /// Starts counting down.
        /// </summary>
        public void Start()
        {
            _start_time = Clock.Time;

            IsTicking = true;
        }

        /// <summary>
        /// Updates the timer's state based on the current time.
        /// </summary>
        /// <returns>
        /// True iff the timer is active and time's up.
        /// </returns>
        public bool Update()
        {
            if (!IsTicking) { return false; }

            ElapsedDuration = Clock.Time - _start_time;
            RemainingDuration = TotalDuration - ElapsedDuration;
            IsRinging = RemainingDuration <= 0;

            return IsRinging;
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(Timer)}{{ " +
                   $"{nameof(IsTicking)} = {IsTicking}, " +
                   $"{nameof(IsRinging)} = {IsRinging}, " +
                   $"{nameof(TotalDuration)} = {TotalDuration}, " +
                   $"{nameof(RemainingDuration)} = {RemainingDuration} }}";
        }

        private float _start_time;
    }
}
