namespace Core.GameStates.States
{
    public class CheckCardsValid : BasicMonoGameState
    {
        public CheckCardsValid(GameContext context) : base(context)
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
                _context.ChangeState(_context.PlayCardState);
            }
            else
            {
                _context.ChangeState(_context.DropCardState);
            }
        }
    }
}