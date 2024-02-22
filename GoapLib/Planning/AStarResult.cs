using System.Collections.Generic;
using GoapLib.Actions;

namespace GoapLib.Planning;

public class AStarResult<TK, TV>
{
    public AStarNode<TK, TV> current;
    public List<Action<TK, TV>> path;
    public bool success;
}