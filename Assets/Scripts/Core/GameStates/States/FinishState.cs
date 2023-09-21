using Library.GameFlow.StateSystem;
using UnityEngine;

namespace Core.GameStates.States
{
    public class FinishState : BasicMonoGameState
    {
        public FinishState(GameContext context, IStateSwitcher stateSwitcher) : base(context, stateSwitcher)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            Debug.Log("Game over!");
        }
    }
}