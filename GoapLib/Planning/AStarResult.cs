using GoapLib.Actions;
using System.Collections.Generic;

namespace GoapLib.Planning;

public class AStarResult<TK, TV>
{
    public AStarNode<TK, TV> current;
    public List<Action<TK, TV>> path;
    public bool success;
}