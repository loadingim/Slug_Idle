public interface IState
{
    public void Enter();
    public void Update();
    public void Exit();
}

public class BaseState : IState
{
    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
