using System;
using DG.Tweening;
using Game;
using UnityEngine;

namespace UI
{
    public class PlayerPawnVisualiser : MonoBehaviour
    {
        [SerializeField] private float animationDuration;

        public event Action OnFinishedMoving;
        
        private void Start()
        {
            GameMaster.Instance.OnGameStarted += GameStarted;
        }

        private void GameStarted()
        {
            GameMaster.Instance.Player.OnPlayerMove += MovePlayer;
            GameMaster.Instance.OnPlacementSuccessful += () => GameMaster.Instance.MoveToNextRoom();
        }

        private void MovePlayer(Vector2Int moveTo)
        {
            transform
                .DOMove(new Vector3(moveTo.x, moveTo.y), animationDuration).OnComplete(FinishMoving);
        }

        private void FinishMoving()
        {
            OnFinishedMoving?.Invoke();
        }
    }
}