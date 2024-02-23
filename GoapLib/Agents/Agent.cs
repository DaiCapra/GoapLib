namespace GoapLib;

public class Agent<TK, TV>
{
    public State<TK, TV> memory;

    public Agent()
    {
        memory = new();
    }
}