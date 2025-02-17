using UnityEngine;

public abstract class Command
{
    public abstract void Execute();
    public abstract bool TryExucute();
    public abstract void GetTurnType();
}
