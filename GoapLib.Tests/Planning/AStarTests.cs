using System.Collections.Generic;
using GoapLib.States;
using NUnit.Framework;

namespace GoapLib.Tests.Planning
{
    [TestFixture]
    public class AStarTests
    {
        [SetUp]
        public void Setup()
        {
            _actions = new List<GameAction>();

            var buyBeans = new GameAction()
                .AddCondition(Attributes.HasMoney, true)
                .AddEffect(Attributes.HasBeans, true)
                .Cast<GameAction>();

            var makeCoffee = new GameAction()
                .AddCondition(Attributes.HasBeans, true)
                .AddEffect(Attributes.HasCoffee, true)
                .Cast<GameAction>();


            var drinkCoffee = new GameAction()
                .AddCondition(Attributes.HasCoffee, true)
                .AddEffect(Attributes.IsThirsty, false)
                .Cast<GameAction>();

            _actions.Add(buyBeans);
            _actions.Add(makeCoffee);
            _actions.Add(drinkCoffee);
        }

        private List<GameAction> _actions;

        [Test]
        public void Test1()
        {
            var start = new State<Attributes, bool>
            {
                [Attributes.HasMoney] = true,
                [Attributes.IsThirsty] = true
            };

            var end = new State<Attributes, bool>
            {
                [Attributes.IsThirsty] = false
            };
        }
    }
}