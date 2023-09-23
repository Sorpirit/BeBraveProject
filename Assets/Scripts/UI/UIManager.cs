using Core.GameStates;
using Game;
using UI.CardsUI;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private PlacementPreview _placementPreview;
        [SerializeField] private PlayerHandUIManager _playerHandUIManager;
        [SerializeField] private GameObject startGamePanel;
        [SerializeField] private GameObject gameOverPanel;
        
        
        private void Awake()
        {
            GameRunner.Instance.OnGameInitFinished += GameInitFinished;

            _playerHandUIManager.OnSelectionChanged += _placementPreview.UpdatePreviews;
        }

        private void GameInitFinished(GameContext context, GameCommander commander)
        {
            commander.PlayCardState.OnStateEnter += () => _placementPreview.UpdatePreviews(_playerHandUIManager.SelectedCardIndex);
            
            commander.PlaceRoomState.OnStateEnter += () => commander.PlaceRoomState.Trigger();
            commander.GameStartState.OnStateEnter += GameStart;

            gameOverPanel.SetActive(false);
            startGamePanel.SetActive(true);
            
            commander.FinishGame.OnStateEnter += () => gameOverPanel.SetActive(true);
        }

        private void GameStart()
        {
            startGamePanel.SetActive(false);
            GameContext context = GameRunner.Instance.Context;
            context.Hand.OnCardAdded += (card) => GameRunner.Instance.Commander.TakeCardState.Trigger();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !GameRunner.Instance.Context.IsGameStarted)
                StartGame();
        }

        public void StartGame()
        {
            startGamePanel.SetActive(false);
            GameRunner.Instance.StartGame();
        }
    }
}