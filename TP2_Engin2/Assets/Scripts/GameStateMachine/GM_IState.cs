public interface GM_IState
{
    public void OnStart();
    public void OnEnter();
    public void OnUpdate();
    public void OnFixedUpdate();
    public void OnExit();
    public bool CanEnter(GM_IState currentState);
    public bool CanExit();
}