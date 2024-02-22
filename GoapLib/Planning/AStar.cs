using System.Collections.Generic;
using System.Linq;
using GoapLib.Actions;
using GoapLib.States;
using Priority_Queue;

namespace GoapLib.Planning;

public class AStar<TK, TV>
{
    private readonly List<Action<TK, TV>> _actions;
    private readonly int _capacity;
    private readonly FastPriorityQueue<AStarNode<TK, TV>> _priorityQueue;
    private readonly Dictionary<int, AStarNode<TK, TV>> _searchSpace;

    private readonly Dictionary<int, AStarNode<TK, TV>> closed;
    private readonly Dictionary<int, AStarNode<TK, TV>> open;

    public AStar(List<Action<TK, TV>> actions)
    {
        _actions = actions;

        var capacity = actions.Count() * 2;
        _capacity = capacity;

        _priorityQueue = new(capacity);
        _searchSpace = new();
        open = new();
        closed = new();
    }

    public AStarNode<TK, TV> GetOrMake(State<TK, TV> state, AStarNode<TK, TV> parent = null)
    {
        if (_searchSpace.TryGetValue(state.GetHashCode(), out var node))
        {
            return node;
        }

        node = new()
        {
            state = state,
            parent = parent
        };

        _searchSpace[state.GetHashCode()] = node;
        return node;
    }

    public AStarResult Run(State<TK, TV> origin, State<TK, TV> goal)
    {
        var result = new AStarResult();

        var start = GetOrMake(origin);
        _priorityQueue.Enqueue(start, start.GetF());
        open.Add(start.Hash, start);

        AStarNode<TK, TV> current = null;

        int iterations = 0;
        while (iterations < _capacity)
        {
            if (open.Count == 0)
            {
                break;
            }

            current = _priorityQueue.Dequeue();
            closed.Add(current.Hash, current);
            open.Remove(current.Hash);

            if (current.state.CanApply(goal))
            {
                result.success = true;
                break;
            }

            var adjacentNodes = GetAdjacent(current);
            foreach (var adjacent in adjacentNodes)
            {
                if (closed.ContainsKey(adjacent.Hash))
                {
                    // Skip if adjacent node is already in closed.
                    continue;
                }

                // Todo: Heuristics
                if (!open.ContainsKey(adjacent.Hash))
                {
                    open.Add(adjacent.Hash, adjacent);
                    _priorityQueue.Enqueue(adjacent, adjacent.GetF());
                }
                else
                {
                    var nodePrevAdded = open[adjacent.Hash];
                    if (adjacent.g < nodePrevAdded.g)
                    {
                        nodePrevAdded.g = adjacent.g;
                        nodePrevAdded.h = adjacent.h;
                        nodePrevAdded.parent = current;

                        _priorityQueue.UpdatePriority(nodePrevAdded, nodePrevAdded.GetF());
                    }
                }
            }

            iterations++;
        }

        return result;
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
            
            if (state.GetHashCode() == currentState.GetHashCode())
            {
                // Skip if next state is the same as current state.
                continue;
            }

            var adjacent = GetOrMake(state, node);
            list.Add(adjacent);
        }

        return list;
    }
}