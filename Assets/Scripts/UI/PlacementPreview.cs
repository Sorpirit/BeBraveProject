using System.Collections.Generic;
using Core.CardSystem.Data.Cards;
using Core.Data;
using Game;
using Game.Data;
using Library.Collections;
using Scripts.DependancyInjector;
using UnityEngine;

namespace UI
{
    public class PlacementPreview : MonoBehaviour
    {
        [SerializeField] private PlacementPreviewCard previewRoom;
        [SerializeField] private RoomOrientationSetupSO roomOrientationSetupSo;
        
        [Inject]
        private IRoomPositionConvertor _positionConvertor;
        
        private PlacementPreviewCard[] _previews;

        private Dictionary<NodeConnections, Sprite> _roomSprites;
        
        private void Awake()
        {
            _roomSprites = roomOrientationSetupSo.RoomSprites;
            InitPreviews(4);
        }

        public void UpdatePreviews(int? cardIndex)
        {
            ResetPreviews();

            if (GameRunner.Instance.Context.Hand.Cards.Count <= 0 || !cardIndex.HasValue)
                return;

            var card = GameRunner.Instance.Context.Hand.GetCard(cardIndex.Value) as RoomCard;
            
            if(card == null)
                return;
            
            var availablePlaces =
                GameRunner.Instance.Context.Map.GetAvailablePlacesAt(GameRunner.Instance.Context.Player.Position,
                    card.Connections);
            for (int i = 0; i < availablePlaces.Length; i++)
            {
                var position = availablePlaces[i];
                _previews[i].transform.position = _positionConvertor.TileToWorld(position);
                bool invertSprite = card.Connections.HasFlag(NodeConnections.Right) && !card.Connections.HasFlag(NodeConnections.Left);
                _previews[i].SetSprite(_roomSprites[card.Connections], invertSprite);
                _previews[i].gameObject.SetActive(true);
            }
        }

        private void InitPreviews(int count)
        {
            _previews = new PlacementPreviewCard[count];
            for (int i = 0; i < count; i++)
            {
                _previews[i] = Instantiate(previewRoom.gameObject, Vector3.zero, Quaternion.identity, transform).GetComponent<PlacementPreviewCard>();
                _previews[i].gameObject.SetActive(false);
            }
        }

        private void ResetPreviews()
        {
            for (int i = 0; i < _previews.Length; i++)
            {
                _previews[i].gameObject.SetActive(false);
            }
        }
    }
}