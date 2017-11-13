using NUnit.Framework;

namespace rharel.M3PD.CouplesTherapyExample.Time.Tests
{
    [TestFixture]
    public sealed class VariableIncrementClockTest
    {
        private static readonly float EPSILON = 0.001f;

        private VariableIncrementClock _clock;

        [SetUp]
        public void Setup()
        {
            _clock = new VariableIncrementClock();
        }

        [Test]
        public void Test_InitialState()
        {
            Assert.AreEqual(0.0f, _clock.Time);
            Assert.AreEqual(0.0f, _clock.TimeIncrement);
        }

        [Test]
        public void Test_Tick()
        {
            _clock.Tick(1.0f);

            Assert.AreEqual(1.0f, _clock.Time, EPSILON);
            Assert.AreEqual(1.0f, _clock.TimeIncrement, EPSILON);
        }

        [Test]
        public void Test_Equality()
        {
            var original = new VariableIncrementClock();
            var good_copy = new VariableIncrementClock();

            var flawed_time_copy = new VariableIncrementClock();
            flawed_time_copy.Tick();

            Assert.AreNotEqual(original, null);
            Assert.AreNotEqual(original, "incompatible type");
            Assert.AreNotEqual(original, flawed_time_copy);

            Assert.AreEqual(original, original);
            Assert.AreEqual(original, good_copy);
        }
    }
}
