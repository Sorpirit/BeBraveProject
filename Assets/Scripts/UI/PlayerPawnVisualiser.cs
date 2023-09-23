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
        [SerializeField] private Ease animationEase;
        
        [Inject]
        private IRoomPositionConvertor _positionConvertor;
        
        private void Awake()
        {
            GameRunner.Instance.OnGameInitFinished += GameInitFinished;
        }

        private void GameInitFinished(GameContext context, GameCommander commander)
        {
            commander.PlayerMoveState.OnStateEnter += MovePlayer;
        }

        private void MovePlayer()
        {
            Assert.IsTrue(GameRunner.Instance.Context.CurrentRoom.HasValue);
            Assert.IsTrue(_positionConvertor != null);
            var moveTo = _positionConvertor.TileToWorld(GameRunner.Instance.Context.CurrentRoom.Value.Position);
            transform
                .DOMove(moveTo, animationDuration)
                .SetEase(animationEase)
                .OnComplete(FinishMoving);
        }

        private void FinishMoving()
        {
            GameRunner.Instance.Commander.PlayerMoveState.Trigger();
        }
    }
}