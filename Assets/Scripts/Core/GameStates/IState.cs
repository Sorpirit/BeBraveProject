using System;

namespace Core.GameStates
{
    public interface IState
    {
        event Action OnStateEnter;
        event Action OnStateExit;

        void EnterState();
        void ExitState();
    }
}