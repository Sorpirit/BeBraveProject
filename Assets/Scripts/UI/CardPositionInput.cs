using Core.GameStates;
using Game;
using UnityEngine;

namespace UI
{
    public class CardPositionInput : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private GameObject coursewareVisualiser;

        private bool _isPlacingRoom;

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
            if(!_isPlacingRoom)
                return;
            
            var mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition += Vector3.one * 0.5f;
            var tilePosition = new Vector2Int(Mathf.FloorToInt(mouseWorldPosition.x),
                Mathf.FloorToInt(mouseWorldPosition.y));

            coursewareVisualiser.transform.position = new Vector3(tilePosition.x, tilePosition.y);

            if (Input.GetMouseButtonDown(0))
            {
                GameRunner.Instance.Context.PlayCardState.PlaceRoom(tilePosition, 0);
            }
        }
    }
}