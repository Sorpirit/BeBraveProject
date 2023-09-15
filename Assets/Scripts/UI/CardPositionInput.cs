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
        [SerializeField] private GameObject coursewareVisualiser;
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
        }

        private void Update()
        {
            var mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int tilePosition = _positionConvertor.WorldToTile(mouseWorldPosition);
            
            coursewareVisualiser.transform.position = Vector3.SmoothDamp(coursewareVisualiser.transform.position, new Vector3(tilePosition.x, tilePosition.y), ref _cursorSpeed, _cursorSpeedAmplitude * Time.deltaTime); 

            if(!_isPlacingRoom)
                return;

            int? selected = _playerHandUIManager.SelectedCardIndex;
            if (Input.GetMouseButtonDown(0) && selected.HasValue)
            {
                GameRunner.Instance.Context.PlayCardState.PlaceRoom(tilePosition, selected.Value);
            }
        }
    }
}