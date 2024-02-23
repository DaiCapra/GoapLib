using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GoapLib.Tests.Planning
{
    [TestFixture]
    public class AStarTests
    {
        private List<GameAction> _actions;

        private GameAction _buyBeans;

        private GameAction _buyCoffee;

        private GameAction _drinkCoffee;

        private GameAction _makeCoffee;

        private List<Action<Attributes, bool>> Actions => _actions
            .Cast<Action<Attributes, bool>>()
            .ToList();

        [Test]
        public void CanPassStress()
        {
            _actions.Add(_buyBeans);
            _actions.Add(_makeCoffee);
            _actions.Add(_drinkCoffee);

            var sw = new Stopwatch();
            sw.Start();

            var size = 10000;

            var list = new List<AStarResult<Attributes, bool>>();
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
            System.Console.WriteLine($"Total: {sw.Elapsed}, Average: {average} ms");

            list.ForEach(result => Assert.IsTrue(result.success));
        }

        [Test]
        public void CanSearch()
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

            var astar = new AStar<Attributes, bool>(Actions);
            var result = astar.Run(start, end);
            Assert.True(result.success);
        }

        [Test]
        public async Task CanSearchOnMultipleThreads()
        {
            _actions.Add(_buyBeans);
            _actions.Add(_makeCoffee);
            _actions.Add(_drinkCoffee);

            var sw = new Stopwatch();
            sw.Start();

            var size = 10000;

            var tasks = new List<Task<AStarResult<Attributes, bool>>>();
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

                var task = Task.Run(() =>
                {
                    var astar = new AStar<Attributes, bool>(Actions);
                    var result = astar.Run(start, end);
                    return result;
                });
                tasks.Add(task);
            }

            var results = await Task.WhenAll(tasks);
            sw.Stop();

            var average = sw.Elapsed.Milliseconds / size;
            System.Console.WriteLine($"Total: {sw.Elapsed}, Average: {average} ms");

            results.ToList().ForEach(result => Assert.IsTrue(result.success));
        }

        [Test]
        public void CanSearchWithHeuristics()
        {
            _actions.Add(_buyBeans);
            _actions.Add(_makeCoffee);
            _actions.Add(_buyCoffee);
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

            var astar = new AStar<Attributes, bool>(Actions);
            var result = astar.Run(start, end);

            Assert.True(result.success);
            Assert.That(result.path.Count, Is.EqualTo(3));
            Assert.That(result.path.Contains(_makeCoffee));

            _makeCoffee.cost = 20;

            astar.Clear();
            result = astar.Run(start, end);
            Assert.True(result.success);
            Assert.That(result.path.Count, Is.EqualTo(2));
            Assert.That(result.path.Contains(_buyCoffee));
        }

        [SetUp]
        public void Setup()
        {
            _actions = new List<GameAction>();

            _buyBeans = new GameAction()
                .AddName("Buy beans")
                .AddCondition(Attributes.HasMoney, true)
                .AddEffect(Attributes.HasBeans, true)
                .AddEffect(Attributes.HasMoney, false)
                .Cast<GameAction>();

            _makeCoffee = new GameAction()
                .AddName("Make coffee")
                .AddCondition(Attributes.HasBeans, true)
                .AddEffect(Attributes.HasCoffee, true)
                .AddEffect(Attributes.HasBeans, false)
                .Cast<GameAction>();

            _buyCoffee = new GameAction()
                .AddName("Buy coffee")
                .AddCondition(Attributes.HasMoney, true)
                .AddEffect(Attributes.HasCoffee, true)
                .AddEffect(Attributes.HasMoney, false)
                .AddCost(10)
                .Cast<GameAction>();

            _drinkCoffee = new GameAction()
                .AddName("Drink coffee")
                .AddCondition(Attributes.HasCoffee, true)
                .AddEffect(Attributes.IsThirsty, false)
                .AddEffect(Attributes.HasCoffee, false)
                .Cast<GameAction>();
        }
    }
}