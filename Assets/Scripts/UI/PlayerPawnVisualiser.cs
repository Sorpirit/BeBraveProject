using Core.GameStates;
using DG.Tweening;
using Game;
using UnityEngine;
using UnityEngine.Assertions;

namespace UI
{
    public class PlayerPawnVisualiser : MonoBehaviour
    {
        [SerializeField] private float animationDuration;

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
            var moveTo = GameRunner.Instance.Context.CurrentRoom.Value.Position;
            transform
                .DOMove(new Vector3(moveTo.x, moveTo.y), animationDuration).OnComplete(FinishMoving);
        }

        private void FinishMoving()
        {
            GameRunner.Instance.Context.PlayerMoveState.Trigger();
        }
    }
}