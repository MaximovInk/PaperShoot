public abstract class Ability
{
    public abstract void Init(Character character);

    public virtual void Update() { }

    public virtual void FixedUpdate() { }

    public void SendCommand(params object[] commands) 
    {
        for (int i = 0; i < commands.Length; i++)
        {
            if (commands != null)
                ExecuteCommand(commands[i]);
        }
    }

    protected virtual void ExecuteCommand(object command) { }
}

public class AxisInput
{
    public float X { get; set; }
    public float Y { get; set; }

    public AxisInput(float x, float y)
    {
        X = x;
        Y = y;
    }
}
