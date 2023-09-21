using Library.GameFlow.StateSystem;
using UnityEngine.Assertions;

namespace Core.GameStates.States
{
    public class RoomContentPlacerState : TriggerGameState
    {
        public IState NextState { get; set; }
        
        protected override IState _nextState => NextState;

        public RoomContentPlacerState(GameContext context, IStateSwitcher stateSwitcher) : base(context, stateSwitcher)
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