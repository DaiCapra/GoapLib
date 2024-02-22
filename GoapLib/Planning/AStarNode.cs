using GoapLib.States;
using Priority_Queue;

namespace GoapLib.Planning;

public class AStarNode<TK, TV> : FastPriorityQueueNode
{
    public float g;
    public float h;

    public AStarNode<TK, TV> parent;
    public State<TK, TV> state;
    public int Hash => state.GetHashCode();

    public float GetF()
    {
        return g + h;
    }
}