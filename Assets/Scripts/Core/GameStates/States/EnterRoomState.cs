using UnityEngine.Assertions;

namespace Core.GameStates.States
{
    public class EnterRoomState : TriggerGameState
    {
        protected override IState _nextState => _context.TakeCardState;
        
        public EnterRoomState(GameContext context) : base(context)
        {
        }

        public override void EnterState()
        {
            Assert.IsTrue(_context.CurrentRoomContent != null);
            _context.CurrentRoomContent.Enter(_context.Player);
            base.EnterState();
        }
    }
}