using UnityEngine.Assertions;

namespace Core.GameStates.States
{
    public class EnterRoomState : BasicMonoGameState, ITriggerTransition
    {
        private IState _nextState => _context.TakeCardState;
        private IState _finishGame => _context.FinishGame;
        
        public EnterRoomState(GameContext context) : base(context)
        {
        }

        public override void EnterState()
        {
            Assert.IsTrue(_context.CurrentRoomContent != null);
            _context.CurrentRoomContent.Enter(_context.Player);
            base.EnterState();
        }
        
        public virtual void Trigger()
        {
            Assert.IsTrue(_isStateActive);
            Assert.IsNotNull(_nextState);
            Assert.IsNotNull(_finishGame);
            _context.ChangeState(_context.Player.HealthSystem.IsDead ? _finishGame : _nextState);
        }
    }
}