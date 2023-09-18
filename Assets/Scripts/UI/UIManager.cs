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
            GameRunner.Instance.OnGameContextCreated += GameContextCreated;

            _playerHandUIManager.OnSelectionChanged += _placementPreview.UpdatePreviews;
        }

        private void GameContextCreated(GameContext context)
        {
            context.PlayCardState.OnStateEnter += () => _placementPreview.UpdatePreviews(_playerHandUIManager.SelectedCardIndex);
            
            context.PlaceRoomState.OnStateEnter += () => context.PlaceRoomState.Trigger();
            context.GameStartState.OnStateEnter += GameStart;

            gameOverPanel.SetActive(false);
            startGamePanel.SetActive(true);
            
            context.FinishGame.OnStateEnter += () => gameOverPanel.SetActive(true);
        }

        private void GameStart()
        {
            GameContext context = GameRunner.Instance.Context;
            context.Hand.OnCardAdded += (card) => context.TakeCardState.Trigger();
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