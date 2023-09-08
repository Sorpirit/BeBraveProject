namespace Core.GameStates
{
    public interface IStatesContext
    {
        void ChangeState(IState state);
    }
}