using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace GoapLib.Tests.Planning
{
    [TestFixture]
    public class PlannerTests
    {
        [SetUp]
        public void Setup()
        {
            _buyBeans = ActionFactory.BuyBeans();
            _makeCoffee = ActionFactory.MakeCoffee();
            _buyCoffee = ActionFactory.BuyCoffee();
            _drinkCoffee = ActionFactory.DrinkCoffee();

            _actions = new();
        }

        private List<Action<Attributes, bool>> _actions;
        private Action<Attributes, bool> _buyBeans;
        private Action<Attributes, bool> _makeCoffee;
        private Action<Attributes, bool> _buyCoffee;
        private Action<Attributes, bool> _drinkCoffee;

        [Test]
        public void Foo()
        {
        }
    }
}