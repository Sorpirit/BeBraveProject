using Core.GameStates;
using Game;
using UnityEngine;

namespace UI
{
    public class CardDropUI : MonoBehaviour
    {
        private bool _isDroppingCard;

        private void Awake()
        {
            GameRunner.Instance.OnGameContextCreated += GameContextCreated;
        }

        private void GameContextCreated(GameContext context)
        {
            context.DropCardState.OnStateEnter += () => _isDroppingCard = true;
            context.DropCardState.OnStateExit += () => _isDroppingCard = false;
        }

        private void Update()
        {
            if(!_isDroppingCard)
                return;
            
            if (Input.GetMouseButtonDown(0))
            {
                GameRunner.Instance.Context.DropCardState.DropCard(0);
            }
        }
    }
}