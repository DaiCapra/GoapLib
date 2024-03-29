namespace GoapLib;

public class Action<TK, TV>
{
    public readonly State<TK, TV> conditions;
    public readonly State<TK, TV> effects;
    public float cost;
    public string name;

    public Action()
    {
        conditions = new();
        effects = new();
    }

    public Action<TK, TV> AddCondition(TK key, TV value)
    {
        conditions[key] = value;
        return this;
    }

    public Action<TK, TV> AddCost(float cost)
    {
        this.cost = cost;
        return this;
    }

    public Action<TK, TV> AddEffect(TK key, TV value)
    {
        effects[key] = value;
        return this;
    }

    public Action<TK, TV> AddName(string name)
    {
        this.name = name;
        return this;
    }

    public T Cast<T>() where T : Action<TK, TV>
    {
        return this as T;
    }

    public override string ToString()
    {
        return name;
    }
}