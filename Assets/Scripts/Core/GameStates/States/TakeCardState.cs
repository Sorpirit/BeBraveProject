using System;
using Library.GameFlow.StateSystem;
using UnityEngine;

namespace Core.GameStates.States
{
    public class TakeCardState : TriggerGameState
    {
        public IState FinishGame { get; set; }
        public IState NextState { get; set; }

        protected override IState _nextState => NextState;
        
        public TakeCardState(GameContext context, IStateSwitcher stateSwitcher) : base(context, stateSwitcher)
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
                _stateSwitcher.ChangeState(FinishGame);
            }
            else
            {
                Trigger();
            }
        }

        
    }
}