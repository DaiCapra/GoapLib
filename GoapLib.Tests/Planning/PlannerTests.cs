using NUnit.Framework;
using System.Collections.Generic;

namespace GoapLib.Tests
{
    [TestFixture]
    public class PlannerTests
    {
        private List<Action<Attributes, bool>> _actions;

        private Action<Attributes, bool> _buyBeans;

        private Action<Attributes, bool> _buyCoffee;

        private Action<Attributes, bool> _drinkCoffee;

        private Action<Attributes, bool> _makeCoffee;

        private Planner<Attributes, bool> _planner;

        [Test]
        public void CanValidateActions()
        {
            _actions.Add(_buyBeans);
            _actions.Add(_makeCoffee);
            _actions.Add(_drinkCoffee);

            var start = new State<Attributes, bool>
            {
                [Attributes.HasMoney] = true,
                [Attributes.IsThirsty] = true
            };

            var end = new State<Attributes, bool>
            {
                [Attributes.IsThirsty] = false
            };

            var astar = new AStar<Attributes, bool>(_actions);
            var result = astar.Run(start, end);
            Assert.True(result.success);

            var plan = result.path;
            Assert.True(_planner.ValidateActions(plan, start));

            start.Set(Attributes.HasMoney, false);
            Assert.False(_planner.ValidateActions(plan, start));
        }

        [SetUp]
        public void Setup()
        {
            _buyBeans = ActionFactory.BuyBeans();
            _makeCoffee = ActionFactory.MakeCoffee();
            _buyCoffee = ActionFactory.BuyCoffee();
            _drinkCoffee = ActionFactory.DrinkCoffee();

            _actions = new();
            _planner = new();
        }
    }
}