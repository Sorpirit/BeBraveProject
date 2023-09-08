using UnityEngine;

namespace Core.GameStates.States
{
    public class DropCardState : BasicMonoGameState, IDropCard
    {
        public DropCardState(GameContext context) : base(context)
        {
        }

        public void DropCard(int i)
        {
            base.EnterState();
            var card = _context.Hand.GetCard(i);
            Debug.Log("Drop card: " + card);
            _context.Hand.DropCard(i);
            _context.Deck.PushBottom(card);
            _context.ChangeState(_context.TakeCardState);
        }
    }
}