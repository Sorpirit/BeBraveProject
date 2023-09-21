using System;

namespace Library.GameFlow.StateSystem
{
    public interface IState
    {
        event Action OnStateEnter;
        event Action OnStateExit;

        void EnterState();
        void ExitState();
    }
}