using GoapLib.Core;
using GoapLib.States;

namespace GoapLib.Actions;

public class Action<TK, TV> : IId
{
    public readonly State<TK, TV> conditions;
    public readonly State<TK, TV> effects;

    public Action()
    {
        conditions = new();
        effects = new();
    }

    public ulong Id { get; set; }

    public Action<TK, TV> AddCondition(TK key, TV value)
    {
        conditions[key] = value;
        return this;
    }

    public Action<TK, TV> AddEffect(TK key, TV value)
    {
        effects[key] = value;
        return this;
    }

    public T Cast<T>() where T : Action<TK, TV>
    {
        return this as T;
    }
}