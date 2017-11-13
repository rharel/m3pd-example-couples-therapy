using Moq;
using NUnit.Framework;
using System;

namespace rharel.M3PD.CouplesTherapyExample.Time.Tests
{
    [TestFixture]
    public sealed class TimerTest
    {
        private static readonly float DURATION = 1.0f;
        private static readonly float HALF_DURATION = 0.5f * DURATION;

        private Mock<Clock> _clock;
        private Timer _timer;

        [SetUp]
        public void Setup()
        {
            _clock = new Mock<Clock>();
            _timer = new Timer(_clock.Object, DURATION);
        }

        [Test]
        public void Test_Constructor_WithInvalidArgs()
        {
            Assert.Throws<ArgumentNullException>(
                () => new Timer(null, 1)
            );
            Assert.Throws<ArgumentException>(
                () => new Timer(_clock.Object, -1)
            );
        }
        [Test]
        public void Test_Constructor()
        {
            Assert.AreSame(_clock.Object, _timer.Clock);

            Assert.AreEqual(DURATION, _timer.TotalDuration);
        }

        [Test]
        public void Test_InitialState()
        {
            Assert.IsFalse(_timer.IsTicking);
            Assert.IsFalse(_timer.IsRinging);

            Assert.AreEqual(DURATION, _timer.RemainingDuration);
            Assert.AreEqual(0.0f, _timer.ElapsedDuration);
        }

        [Test]
        public void Test_Set()
        {
            _timer.Set(HALF_DURATION);

            Assert.AreEqual(HALF_DURATION, _timer.TotalDuration);
            Assert.AreEqual(HALF_DURATION, _timer.RemainingDuration);
            Assert.AreEqual(0.0f, _timer.ElapsedDuration);
        }

        [Test]
        public void Test_Countdown_AtStart()
        {
            _timer.Start();

            Assert.IsTrue(_timer.IsTicking);
        }
        [Test]
        public void Test_Countdown_AfterStartAndBeforeRing()
        {
            _timer.Start();

            _clock
                .SetupGet(c => c.Time)
                .Returns(HALF_DURATION);

            Assert.IsFalse(_timer.Update());
            Assert.IsFalse(_timer.IsRinging);

            Assert.AreEqual(HALF_DURATION, _timer.RemainingDuration);
            Assert.AreEqual(HALF_DURATION, _timer.ElapsedDuration);
        }
        [Test]
        public void Test_Countdown_AtRing()
        {
            _timer.Start();

            _clock
                .SetupGet(c => c.Time)
                .Returns(DURATION);

            Assert.IsTrue(_timer.Update());
            Assert.IsTrue(_timer.IsRinging);

            Assert.AreEqual(0.0f, _timer.RemainingDuration);
            Assert.AreEqual(DURATION, _timer.ElapsedDuration);
        }

        [Test]
        public void Test_Countdown_AfterRingAndReset()
        {
            _timer.Start();

            _clock
                .SetupGet(c => c.Time)
                .Returns(DURATION);

            _timer.Update();
            _timer.Reset();

            Assert.IsFalse(_timer.IsTicking);
            Assert.IsFalse(_timer.IsRinging);

            Assert.AreEqual(DURATION, _timer.RemainingDuration);
            Assert.AreEqual(0.0f, _timer.ElapsedDuration);
        }
    }
}
