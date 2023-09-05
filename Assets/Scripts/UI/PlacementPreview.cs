using System;
using Game;
using UnityEngine;

namespace UI
{
    public class PlacementPreview : MonoBehaviour
    {
        [SerializeField] private GameObject previewRoom;

        private GameObject[] _previews;

        private void Awake()
        {
            InitPreviews(4);
        }
        
        public void UpdatePreviews()
        {
            ResetPreviews();
            
            if (GameMaster.Instance.Hand.Cards.Count <= 0) 
                return;
            
            var card = GameMaster.Instance.Hand.GetCard(0);
            var availablePlaces = GameMaster.Instance.Map.GetAvailablePlacesAt(GameMaster.Instance.Player.Position, card.Connections);
            for (int i = 0; i < availablePlaces.Length; i++)
            {
                var position = availablePlaces[i];
                _previews[i].transform.position = new Vector3(position.x, position.y);
                _previews[i].SetActive(true);
            }
        }

        private void InitPreviews(int count)
        {
            _previews = new GameObject[count];
            for (int i = 0; i < count; i++)
            {
                _previews[i] = Instantiate(previewRoom, Vector3.zero, Quaternion.identity, transform);
                _previews[i].SetActive(false);
            }
        }
        
        private void ResetPreviews()
        {
            for (int i = 0; i < _previews.Length; i++)
            {
                _previews[i].SetActive(false);
            }
        }
    }
}