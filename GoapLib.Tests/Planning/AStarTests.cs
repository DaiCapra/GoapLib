using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GoapLib.Planning;
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
                .AddEffect(Attributes.HasMoney, false)
                .Cast<GameAction>();

            var makeCoffee = new GameAction()
                .AddCondition(Attributes.HasBeans, true)
                .AddEffect(Attributes.HasCoffee, true)
                .AddEffect(Attributes.HasBeans, false)
                .Cast<GameAction>();


            var drinkCoffee = new GameAction()
                .AddCondition(Attributes.HasCoffee, true)
                .AddEffect(Attributes.IsThirsty, false)
                .AddEffect(Attributes.HasCoffee, false)
                .Cast<GameAction>();

            _actions.Add(buyBeans);
            _actions.Add(makeCoffee);
            _actions.Add(drinkCoffee);
        }

        private List<GameAction> _actions;

        private List<Actions.Action<Attributes, bool>> Actions => _actions
            .Cast<Actions.Action<Attributes, bool>>()
            .ToList();

        [Test]
        public void CanSearch()
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

            var astar = new AStar<Attributes, bool>(Actions);
            var result = astar.Run(start, end);
            Assert.True(result.success);
        }

        [Test]
        public void CanPassStress()
        {
            var sw = new Stopwatch();
            sw.Start();
            
            var size = 10000;

            var list = new List<AStarResult>();
            for (int i = 0; i < size; i++)
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

                var astar = new AStar<Attributes, bool>(Actions);
                var result = astar.Run(start, end);
                list.Add(result);
            }

            sw.Stop();
            
            var average = sw.Elapsed.Milliseconds / size;
            Console.WriteLine($"Total: {sw.Elapsed}, Average: {average} ms");
            
            list.ForEach(result => Assert.IsTrue(result.success));
        }
    }
}