using GoapLib.Actions;
using GoapLib.States;
using Priority_Queue;

namespace GoapLib.Planning;

public class AStarNode<TK, TV> : FastPriorityQueueNode
{
    public float g;
    public float h;

    public Action<TK, TV> parentAction;
    public AStarNode<TK, TV> parentNode;
    public State<TK, TV> state;

    public int Hash => state.GetHashCode();

    public float GetF()
    {
        return g + h;
    }
}