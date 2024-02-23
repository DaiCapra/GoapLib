using NUnit.Framework;

namespace GoapLib.Tests.Planning
{
    [TestFixture]
    public class StateTests
    {
        [Test]
        public void DoHashesDiffer()
        {
            var state1 = new State<Attributes, bool>
            {
                [Attributes.HasBeans] = true,
                [Attributes.HasCoffee] = true
            };

            var state2 = new State<Attributes, bool>
            {
                [Attributes.HasMoney] = true,
                [Attributes.IsThirsty] = true
            };

            Assert.AreNotEqual(state1.GetHashCode(), state2.GetHashCode());
        }

        [Test]
        public void DoHashesEqual()
        {
            var state1 = new State<Attributes, bool>
            {
                [Attributes.HasBeans] = true,
                [Attributes.HasCoffee] = true
            };

            var state2 = new State<Attributes, bool>
            {
                [Attributes.HasBeans] = true,
                [Attributes.HasCoffee] = true
            };

            Assert.AreEqual(state1.GetHashCode(), state2.GetHashCode());
        }
    }
}