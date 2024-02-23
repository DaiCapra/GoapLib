using System.Collections.Generic;

namespace GoapLib;

public class AStarResult<TK, TV>
{
    public float cost;
    public AStarNode<TK, TV> last;
    public List<Action<TK, TV>> path;
    public bool success;
}