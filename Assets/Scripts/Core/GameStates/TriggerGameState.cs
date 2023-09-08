using UnityEngine.Assertions;

namespace Core.GameStates
{
    public abstract class TriggerGameState : BasicMonoGameState, ITriggerTransition
    {
        protected abstract IState _nextState { get; }

        public TriggerGameState(GameContext context) : base(context) { }

        public virtual void Trigger()
        {
            Assert.IsTrue(_isStateActive);
            Assert.IsNotNull(_nextState);
            _context.ChangeState(_nextState);
        }
    }
}