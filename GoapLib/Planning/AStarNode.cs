using Priority_Queue;

namespace GoapLib;

public class AStarNode<TK, TV> : FastPriorityQueueNode
{
    public float g;
    public float h;

    public Action<TK, TV> parentAction;
    public AStarNode<TK, TV> parentNode;
    public State<TK, TV> state;

    public string Hash => state.hash;

    public float GetF()
    {
        return g + h;
    }
}