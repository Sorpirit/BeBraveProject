using UnityEngine;

namespace Core.GameStates.States
{
    public class FinishState : BasicMonoGameState
    {
        public FinishState(GameContext context) : base(context)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            Debug.Log("Game over!");
        }
    }
}