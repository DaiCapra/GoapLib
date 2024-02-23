using System.Collections.Generic;

namespace GoapLib;

public class Planner<TK, TV>
{
    public AStarResult<TK, TV> Plan(
        State<TK, TV> currentState,
        State<TK, TV> goal,
        List<Action<TK, TV>> actions
    )
    {
        var astar = new AStar<TK, TV>(actions);
        var result = astar.Run(currentState, goal);
        return result;
    }

    public bool ValidateActions(List<Action<TK, TV>> actions, State<TK, TV> state)
    {
        var current = state.Copy();
        foreach (var action in actions)
        {
            if (!current.CanApply(action.conditions))
            {
                return false;
            }

            current.Apply(action.effects);
        }

        return true;
    }
}