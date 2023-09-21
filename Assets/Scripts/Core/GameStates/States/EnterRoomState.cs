using Library.GameFlow.StateSystem;
using UnityEngine.Assertions;

namespace Core.GameStates.States
{
    public class EnterRoomState : BasicMonoGameState, ITriggerTransition
    {
        public IState NextState { get; set; }

        public IState FinishGame { get; set; }
        
        public EnterRoomState(GameContext context, IStateSwitcher stateSwitcher) : base(context, stateSwitcher)
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
            Assert.IsNotNull(NextState);
            Assert.IsNotNull(FinishGame);
            _stateSwitcher.ChangeState(_context.IsGameFinished ? FinishGame : NextState);
        }
    }
}