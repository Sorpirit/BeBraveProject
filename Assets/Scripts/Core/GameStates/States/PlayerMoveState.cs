using UnityEngine.Assertions;

namespace Core.GameStates.States
{
    public class PlayerMoveState : TriggerGameState
    {
        protected override IState _nextState => _context.PlayerEnterRoomState;
        
        public PlayerMoveState(GameContext context) : base(context) { }

        public override void EnterState()
        {
            Assert.IsTrue(_context.CurrentRoom.HasValue);
            _context.Player.Move(_context.CurrentRoom.Value.Position);
            base.EnterState();
        }
    }
}