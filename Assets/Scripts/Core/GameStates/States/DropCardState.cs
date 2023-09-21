using Library.GameFlow.StateSystem;

namespace Core.GameStates.States
{
    public class DropCardState : BasicMonoGameState, IDropCard
    {
        public IState NextState { get; set; }
        
        public DropCardState(GameContext context, IStateSwitcher stateSwitcher) : base(context, stateSwitcher)
        {
        }

        public void DropCard(int i)
        {
            base.EnterState();
            var card = _context.Hand.GetCard(i);
            _context.Hand.DropCard(i);
            _context.Deck.PushBottom(card);
            _stateSwitcher.ChangeState(NextState);
        }
    }
}