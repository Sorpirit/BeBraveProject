namespace Core.GameStates
{
    public interface ITriggerTransition : IState
    {
        void Trigger();
    }
}