using System;
using Game;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private PlayerPawnVisualiser _pawnVisualiser;
        [SerializeField] private PlacementPreview _placementPreview;
        
        private void Start()
        {
            _pawnVisualiser.OnFinishedMoving += EnterRoom;
            GameMaster.Instance.OnGameStarted += () => _placementPreview.UpdatePreviews();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
                GameMaster.Instance.StartGame();
        }

        private void EnterRoom()
        {
            GameMaster.Instance.EnterNextRoom();
            GameMaster.Instance.TakeCard();
            _placementPreview.UpdatePreviews();
        }
    }
}