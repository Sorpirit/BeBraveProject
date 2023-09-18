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
        private Vector3 _cursorSpeed;
        private readonly float _cursorSpeedAmplitude = 18;
        
        private void Awake()
        {
            GameRunner.Instance.OnGameContextCreated += GameContextCreated;
        }

        private void GameContextCreated(GameContext context)
        {
            context.PlayCardState.OnStateEnter += () => _isPlacingRoom = true;
            context.PlayCardState.OnStateExit += () => _isPlacingRoom = false;
            
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
            return GameRunner.Instance.Context.PlayCardState.PlaceRoom(tilePosition, selectedCardIndex);
        }
    }
}