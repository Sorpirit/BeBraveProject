using UnityEngine.Assertions;

namespace Core.GameStates.States
{
    public class RoomContentPlacerState : TriggerGameState
    {
        protected override IState _nextState => _context.PlayerMoveState;
        
        public RoomContentPlacerState(GameContext context) : base(context)
        {
        }

        public override void EnterState()
        {
            Assert.IsTrue(_context.CurrentRoom.HasValue && _context.UsedRoomCard != null);
            _context.CurrentRoomContent = _context.RoomFactory.CreateRoom(_context.UsedRoomCard.RoomId, _context.CurrentRoom.Value);
            base.EnterState();
        }
    }
}