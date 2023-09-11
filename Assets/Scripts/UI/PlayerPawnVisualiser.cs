using Core.GameStates;
using DG.Tweening;
using Game;
using Scripts.DependancyInjector;
using UnityEngine;
using UnityEngine.Assertions;

namespace UI
{
    public class PlayerPawnVisualiser : MonoBehaviour
    {
        [SerializeField] private float animationDuration;

        [Inject]
        private IRoomPositionConvertor _positionConvertor;
        
        private void Awake()
        {
            GameRunner.Instance.OnGameContextCreated += GameContextCreated;
        }

        private void GameContextCreated(GameContext context)
        {
            context.PlayerMoveState.OnStateEnter += MovePlayer;
        }

        private void MovePlayer()
        {
            Assert.IsTrue(GameRunner.Instance.Context.CurrentRoom.HasValue);
            Assert.IsTrue(_positionConvertor != null);
            var moveTo = _positionConvertor.TileToWorld(GameRunner.Instance.Context.CurrentRoom.Value.Position);
            transform
                .DOMove(moveTo, animationDuration).OnComplete(FinishMoving);
        }

        private void FinishMoving()
        {
            GameRunner.Instance.Context.PlayerMoveState.Trigger();
        }
    }
}