using System;
using UnityEngine;

namespace Core.GameStates.States
{
    public class TakeCardState : TriggerGameState
    {
        protected override IState _nextState => _context.PlayCardState;
        
        public TakeCardState(GameContext context) : base(context)
        {
        }
        
        public override void EnterState()
        {
            base.EnterState();
            if (!_context.Hand.CanTakeCard)
            {
                Trigger();
                return;
            }
            
            if (_context.Deck.CardCount > 0)
            {
                _context.Hand.TakeCard(_context.Deck.TakeTop());
            }
            else if(_context.Hand.Cards.Count == 0)
            {
                Debug.Log("Game over!");
                _context.ChangeState(_context.FinishGame);
            }
            else
            {
                Trigger();
            }
        }
    }
}