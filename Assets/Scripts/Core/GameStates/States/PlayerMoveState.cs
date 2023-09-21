using Library.GameFlow.StateSystem;
using UnityEngine.Assertions;

namespace Core.GameStates.States
{
    public class PlayerMoveState : TriggerGameState
    {
        public IState NextState { get; set; }

        protected override IState _nextState => NextState;
        
        public PlayerMoveState(GameContext context, IStateSwitcher stateSwitcher) : base(context, stateSwitcher)
        {
        }
        
        public override void EnterState()
        {
            Assert.IsTrue(_context.CurrentRoom.HasValue);
            _context.Player.Move(_context.CurrentRoom.Value.Position);
            base.EnterState();
        }

        
    }
}