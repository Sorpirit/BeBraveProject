using System;
using Library.GameFlow.StateSystem;

namespace Core.GameStates
{
    public class BasicMonoGameState : IState
    {
        public event Action OnStateEnter;
        public event Action OnStateExit;

        protected bool _isStateActive = false;
        protected GameContext _context;
        protected IStateSwitcher _stateSwitcher;
        
        public BasicMonoGameState(GameContext context, IStateSwitcher stateSwitcher)
        {
            _context = context;
            _stateSwitcher = stateSwitcher;
        }

        public virtual void EnterState()
        {
            _isStateActive = true;
            OnStateEnter?.Invoke();
        }

        public virtual void ExitState()
        {
            _isStateActive = false;
            OnStateExit?.Invoke();
        }
    }
}