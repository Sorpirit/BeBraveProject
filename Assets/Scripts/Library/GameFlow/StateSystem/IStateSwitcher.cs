namespace Library.GameFlow.StateSystem
{
    public interface IStateSwitcher
    {
        void ChangeState(IState state);
    }
}