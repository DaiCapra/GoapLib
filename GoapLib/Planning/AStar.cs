using System.Collections.Generic;
using GoapLib.Actions;
using GoapLib.States;
using Priority_Queue;

namespace GoapLib.Planning;

public class AStar<TK, TV>
{
    public AStarNode GetOrMake(ulong id, AStarNode parent = null)
    {
        if (searchSpace.TryGetValue(id, out var node))
        {
            return node;
        }

        node = new()
        {
            id = id,
            parent = parent
        };
        
        searchSpace[id] = node;
        return node;
    }

    private readonly Dictionary<ulong, Action<TK, TV>> _actions;
    private readonly int _capacity;
    private FastPriorityQueue<AStarNode> _priorityQueue;

    public Dictionary<ulong, AStarNode> closed;
    public Dictionary<ulong, AStarNode> open;
    public Dictionary<ulong, AStarNode> searchSpace;

    public AStar(int capacity, IEnumerable<Action<TK, TV>> actions)
    {
        _capacity = capacity;
        _priorityQueue = new(capacity);

        _actions = new();
        foreach (var action in actions)
        {
            _actions[action.Id] = action;
        }
    }

    public AStarResult Run(State<TK, TV> origin, State<TK, TV> goal)
    {
        var result = new AStarResult();
        
        // var start = GetOrMake(origin);
        // _priorityQueue.Enqueue(start, start.F);
        // open.Add(start.position, start);
        
        int iterations = 0;
        while (iterations < _capacity)
        {
            if (open.Count == 0)
            {
                break;
            }

            iterations++;
        }

        return result;
    }
}