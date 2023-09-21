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
            GameRunner.Instance.OnGameInitFinished += GameInitFinished;
        }

        private void GameInitFinished(GameContext context, GameCommander commander)
        {
            commander.DropCardState.OnStateEnter += () => _isDroppingCard = true;
            commander.DropCardState.OnStateExit += () => _isDroppingCard = false;
        }

        private void Update()
        {
            if(!_isDroppingCard)
                return;
            
            if (Input.GetMouseButtonDown(0))
            {
                GameRunner.Instance.Commander.DropCardState.DropCard(0);
            }
        }
    }
}