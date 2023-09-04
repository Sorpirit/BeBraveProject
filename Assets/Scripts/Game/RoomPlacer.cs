using System;
using Core.Data;
using Core.Data.Scriptable;
using Library.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game
{
    public class RoomPlacer : MonoBehaviour
    {
        [SerializeField] 
        private GameObject roomPrefab;
        [SerializeField] 
        private GameObject roomPreview;
        [SerializeField] 
        private CardSet set;
        
        private Vector2Int _pointer = new(0, 1);

        private bool _isGameStarted;
        private Deck _deck;
        private Dungeon _map;
        private PlayerHand _hand;

        private GameObject[] _previews;

        private void Start()
        {
            StartGame();
            InitPreviews(32);
        }

        private void InitPreviews(int count)
        {
            _previews = new GameObject[count];
            for (int i = 0; i < count; i++)
            {
                _previews[i] = Instantiate(roomPreview, Vector3.zero, Quaternion.identity);
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

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlaceRoom();
            }
        }

        private void StartGame()
        {
            _deck = new Deck(set);
            _map = new Dungeon();
            _hand = new PlayerHand(3);

            _deck.OnCardCountChanged += cardCount => Debug.Log("Deck: " + cardCount);
            _map.OnRoomPlaced += MapOnOnRoomPlaced;
            _hand.OnHandUpdated += cards => Debug.Log("Player Hand: " + string.Join(", ", cards));
            
            _map.InitRoom(Vector2Int.zero);

            Assert.IsTrue(_deck.CardCount >= _hand.HandCapacity);
            while (_hand.CanTakeCard)
            {
                var card = _deck.TakeTop();
                _hand.TakeCard(card);
            }

            _isGameStarted = true;
        }

        private void MapOnOnRoomPlaced(Vector2Int arg1, Room arg2)
        {
            Instantiate(roomPrefab, new Vector3(arg1.x, arg1.y), Quaternion.identity);
        }

        private void PlaceRoom()
        {
            Assert.IsTrue(_isGameStarted);
            var room = _hand.GetCard(0);
            bool result = _map.PlaceRoom(_pointer, new Room(_pointer, room.Room.Connections));
            if (!result)
            {
                Debug.Log("Unable to place room");
                return;
            }
            
            _hand.PlayCard(0);
            if (_hand.CanTakeCard)
            {
                if (_deck.CardCount > 0)
                {
                    _hand.TakeCard(_deck.TakeTop());
                }
                else if(_hand.Cards.Count == 0)
                {
                    Debug.Log("Game over!");
                }
            }

            if (_hand.Cards.Count > 0)
            {
                var card = _hand.GetCard(0);
                var availablePlaces = _map.GetAvailablePlaces(card.Room);
                ResetPreviews();
                for (int i = 0; i < availablePlaces.Length; i++)
                {
                    var position = availablePlaces[i];
                    _previews[i].transform.position = new Vector3(position.x, position.y);
                    _previews[i].SetActive(true);
                }
            }
            
            _pointer += Vector2Int.up;
        }
    }
}