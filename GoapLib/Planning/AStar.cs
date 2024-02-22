using GoapLib.Actions;
using GoapLib.States;
using Priority_Queue;
using System.Collections.Generic;
using System.Linq;

namespace GoapLib.Planning;

public class AStar<TK, TV>
{
    private readonly List<Action<TK, TV>> _actions;
    private readonly int _capacity;

    private readonly Dictionary<string, AStarNode<TK, TV>> _closed;
    private readonly Dictionary<string, AStarNode<TK, TV>> _open;
    private readonly FastPriorityQueue<AStarNode<TK, TV>> _priorityQueue;
    private readonly Dictionary<string, AStarNode<TK, TV>> _searchSpace;

    public AStar(List<Action<TK, TV>> actions)
    {
        _actions = actions;

        var capacity = actions.Count() * 2;
        _capacity = capacity;

        _priorityQueue = new(capacity);
        _searchSpace = new();
        _open = new();
        _closed = new();
    }

    public void Clear()
    {
        _priorityQueue.Clear();
        _searchSpace.Clear();
        _open.Clear();
        _closed.Clear();
    }

    public AStarResult<TK, TV> Run(State<TK, TV> origin, State<TK, TV> goal)
    {
        var start = GetOrMake(origin);
        _priorityQueue.Enqueue(start, start.GetF());
        _open.Add(start.Hash, start);

        var result = Search(goal);
        CalculatePath(result);

        Clear();
        return result;
    }

    private float CalculateH(AStarNode<TK, TV> adjacent, State<TK, TV> goal)
    {
        int cost = 0;

        var map = adjacent.state.map;
        foreach (var kv in goal.map)
        {
            var key = kv.Key;
            var goalValue = kv.Value;

            if (!map.ContainsKey(key) || !map[key].Equals(goalValue))
            {
                cost++;
            }
        }

        return cost;
    }

    private void CalculatePath(AStarResult<TK, TV> result)
    {
        if (!result.success)
        {
            return;
        }

        var path = new List<Action<TK, TV>>();
        var current = result.current;

        while (current != null && current.parentAction != null)
        {
            path.Add(current.parentAction);
            current = current.parentNode;
        }

        path.Reverse();
        result.path = path;
    }

    private List<AStarNode<TK, TV>> GetAdjacent(AStarNode<TK, TV> node)
    {
        var list = new List<AStarNode<TK, TV>>();
        var currentState = node.state;

        foreach (var action in _actions)
        {
            if (!currentState.CanApply(action.conditions))
            {
                // Skip if action cannot be applied to current state.
                continue;
            }

            var state = node.state.Copy();
            state.Apply(action.effects);

            if (state.hash == currentState.hash)
            {
                // Skip if next state is the same as current state.
                continue;
            }

            var adjacent = GetOrMake(state, parentNode: node, parentAction: action);
            list.Add(adjacent);
        }

        return list;
    }

    private AStarNode<TK, TV> GetOrMake(
        State<TK, TV> state,
        AStarNode<TK, TV> parentNode = null,
        Action<TK, TV> parentAction = null
    )
    {
        if (_searchSpace.TryGetValue(state.hash, out var node))
        {
            return node;
        }

        node = new()
        {
            state = state,
            parentNode = parentNode,
            parentAction = parentAction
        };

        _searchSpace[state.hash] = node;
        return node;
    }

    private AStarResult<TK, TV> Search(State<TK, TV> goal)
    {
        var result = new AStarResult<TK, TV>();

        int iterations = 0;
        while (iterations < _capacity)
        {
            if (_open.Count == 0)
            {
                break;
            }

            var current = _priorityQueue.Dequeue();
            _closed.Add(current.Hash, current);
            _open.Remove(current.Hash);

            if (current.state.CanApply(goal))
            {
                result.current = current;
                result.success = true;
                break;
            }

            var adjacentNodes = GetAdjacent(current);
            foreach (var adjacent in adjacentNodes)
            {
                if (_closed.ContainsKey(adjacent.Hash))
                {
                    // Skip if adjacent node is already in closed.
                    continue;
                }

                adjacent.g = adjacent.parentAction.cost;
                adjacent.h = CalculateH(adjacent, goal);

                if (!_open.ContainsKey(adjacent.Hash))
                {
                    _open.Add(adjacent.Hash, adjacent);
                    _priorityQueue.Enqueue(adjacent, adjacent.GetF());
                }
                else
                {
                    var nodePrevAdded = _open[adjacent.Hash];
                    if (adjacent.g < nodePrevAdded.g)
                    {
                        nodePrevAdded.g = adjacent.g;
                        nodePrevAdded.h = adjacent.h;
                        nodePrevAdded.parentNode = current;

                        _priorityQueue.UpdatePriority(nodePrevAdded, nodePrevAdded.GetF());
                    }
                }
            }

            iterations++;
        }

        return result;
    }
}