using System;
using Core.Data;
using Core.Data.Rooms;
using Core.Data.Scriptable;
using Core.PlayerSystems;
using Core.RoomsSystem;
using Library.Collections;
using Scripts.DependancyInjector;
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
        [SerializeField] 
        private GameObject playerGO;

        private bool _isGameStarted;
        private Deck _deck;
        private Dungeon _map;
        private PlayerHand _hand;
        private PlayerPawn _player;

        [Inject]
        private IRoomFactory _roomFactory;
        
        private GameObject[] _previews;

        private void Start()
        {
            InitPreviews(32);
            StartGame();
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
            _player = new PlayerPawn(new PlayerHealth(100));

            _deck.OnCardCountChanged += cardCount => Debug.Log("Deck: " + cardCount);
            _map.OnRoomPlaced += MapOnOnRoomPlaced;
            _hand.OnHandUpdated += cards => Debug.Log("Player Hand: " + string.Join(", ", cards));
            _player.OnPlayerMove += newPos => playerGO.transform.position = new Vector3(newPos.x, newPos.y);
            
            _map.InitRoom(Vector2Int.zero);

            Assert.IsTrue(_deck.CardCount >= _hand.HandCapacity);
            while (_hand.CanTakeCard)
            {
                var card = _deck.TakeTop();
                _hand.TakeCard(card);
            }

            _isGameStarted = true;
            UpdatePreviews();
        }

        private void MapOnOnRoomPlaced(Vector2Int arg1, Room arg2)
        {
            Instantiate(roomPrefab, new Vector3(arg1.x, arg1.y), Quaternion.identity);
        }

        private void PlaceRoom()
        {
            Assert.IsTrue(_isGameStarted);
            
            var roomCard = _hand.GetCard(0);
            Vector2 placementPosition = _previews[0].transform.position;
            var roomPosition = new Vector2Int((int)placementPosition.x, (int)placementPosition.y);
            bool result = _map.PlaceRoom(roomPosition, roomCard, out var room);
            if (!result)
            {
                Debug.Log("Unable to place room: " + roomCard);
                return;
            }
            
            _hand.PlayCard(0);

            var roomContent = _roomFactory.CreateRoom(roomCard.RoomId, room);
            
            _player.Move(roomPosition);
            
            roomContent.Enter(_player);

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
            
            UpdatePreviews();
        }

        private void UpdatePreviews()
        {
            ResetPreviews();
            
            if (_hand.Cards.Count <= 0) 
                return;
            
            var card = _hand.GetCard(0);
            var availablePlaces = _map.GetAvailablePlacesAt(_player.Position, card.Connections);
            for (int i = 0; i < availablePlaces.Length; i++)
            {
                var position = availablePlaces[i];
                _previews[i].transform.position = new Vector3(position.x, position.y);
                _previews[i].SetActive(true);
            }
        }
    }
}