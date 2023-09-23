using Core.GameStates;
using Game;
using Scripts.DependancyInjector;
using UI.CardsUI;
using UnityEngine;

namespace UI
{
    public class CardPositionInput : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private PlayerHandUIManager _playerHandUIManager;
        
        [Inject]
        private IRoomPositionConvertor _positionConvertor;
        
        private bool _isPlacingRoom;
        
        private void Awake()
        {
            GameRunner.Instance.OnGameInitFinished += GameInitFinished;
        }

        private void GameInitFinished(GameContext context, GameCommander commander)
        {
            commander.PlayCardState.OnStateEnter += () => _isPlacingRoom = true;
            commander.PlayCardState.OnStateExit += () => _isPlacingRoom = false;
            
            _playerHandUIManager.TryPlace = TryPlace;
        }

        private void Update()
        {
            if(_isPlacingRoom && Input.GetMouseButtonDown(0) && _playerHandUIManager.SelectedCardIndex.HasValue)
                TryPlace(_playerHandUIManager.SelectedCardIndex.Value);
        }

        private bool TryPlace(int selectedCardIndex)
        {
            if(!_isPlacingRoom)
                return false;
           
            return TryPlace(selectedCardIndex, Input.mousePosition);
        }
        
        private bool TryPlace(int selectedCardIndex, Vector3 pointerScreenPosition)
        {
            if(!_isPlacingRoom)
                return false;
            
            Vector3 pointerWorldPosition = _camera.ScreenToWorldPoint(pointerScreenPosition);
            Vector2Int tilePosition = _positionConvertor.WorldToTile(pointerWorldPosition);
            return GameRunner.Instance.Commander.PlayCardState.PlaceRoom(tilePosition, selectedCardIndex);
        }
    }
}