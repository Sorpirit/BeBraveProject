using Library.GameFlow.StateSystem;

namespace Core.GameStates
{
    public interface ITriggerTransition : IState
    {
        void Trigger();
    }
}