using Core.GameStates;
using Game;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private PlacementPreview _placementPreview;

        private void Awake()
        {
            GameRunner.Instance.OnGameContextCreated += GameContextCreated;
        }

        private void GameContextCreated(GameContext context)
        {
            context.PlayCardState.OnStateEnter += () => _placementPreview.UpdatePreviews();
            
            context.PlaceRoomState.OnStateEnter += () => context.PlaceRoomState.Trigger();
            context.PlayerEnterRoomState.OnStateEnter += () => context.PlayerEnterRoomState.Trigger();
            context.GameStartState.OnStateEnter += GameStart;
        }

        private void GameStart()
        {
            GameContext context = GameRunner.Instance.Context;
            context.Hand.OnCardAdded += (card) => context.TakeCardState.Trigger();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !GameRunner.Instance.Context.IsGameStarted)
                GameRunner.Instance.StartGame();
        }
    }
}