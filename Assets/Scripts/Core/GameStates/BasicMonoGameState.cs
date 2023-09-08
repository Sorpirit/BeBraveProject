using System;

namespace Core.GameStates
{
    public class BasicMonoGameState : IState
    {
        public event Action OnStateEnter;
        public event Action OnStateExit;

        protected bool _isStateActive = false;
        protected GameContext _context;
        
        public BasicMonoGameState(GameContext context)
        {
            _context = context;
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