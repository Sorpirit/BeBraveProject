using Library.GameFlow.StateSystem;
using UnityEngine.Assertions;

namespace Core.GameStates
{
    public abstract class TriggerGameState : BasicMonoGameState, ITriggerTransition
    {
        protected abstract IState _nextState { get; }
        
        protected TriggerGameState(GameContext context, IStateSwitcher stateSwitcher) : base(context, stateSwitcher)
        {
        }

        public virtual void Trigger()
        {
            Assert.IsTrue(_isStateActive);
            Assert.IsNotNull(_nextState);
            _stateSwitcher.ChangeState(_nextState);
        }
    }
}