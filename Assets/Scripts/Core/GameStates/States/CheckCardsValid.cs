using Library.GameFlow.StateSystem;

namespace Core.GameStates.States
{
    public class CheckCardsValid : BasicMonoGameState
    {
        public IState PlayCardState { get; set; }
        public IState DropCardState { get; set; }
        
        public CheckCardsValid(GameContext context, IStateSwitcher stateSwitcher) : base(context, stateSwitcher)
        {
        }
        
        public override void EnterState()
        {
            base.EnterState();
            bool[] validCards = new bool[_context.Hand.Cards.Count];
            bool hasValidCard = false;
            for (int i = 0; i < _context.Hand.Cards.Count; i++)
            {
                var card = _context.Hand.GetCard(i);
            
                var availablePlaces = _context.Map.GetAvailablePlacesAt(_context.Player.Position, card.Connections);
                if (availablePlaces.Length > 0)
                {
                    validCards[i] = true;
                    hasValidCard = true;
                }
            }

            if (hasValidCard)
            {
                _stateSwitcher.ChangeState(PlayCardState);
            }
            else
            {
                _stateSwitcher.ChangeState(DropCardState);
            }
        }
    }
}