using Priority_Queue;

namespace GoapLib.Planning;

public class AStarNode : FastPriorityQueueNode
{
    public float g;
    public float h;
    public ulong id;
    public AStarNode parent;
    public float F => g + h;
}