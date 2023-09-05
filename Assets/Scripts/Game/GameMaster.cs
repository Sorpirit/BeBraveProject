using System;
using Core.Data;
using Core.Data.Rooms;
using Core.Data.Scriptable;
using Core.PlayerSystems;
using Core.RoomsSystem;
using Scripts.DependancyInjector;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game
{
    public class GameMaster : MonoBehaviour
    {
        public static GameMaster Instance { get; private set; }

        [Inject] private IRoomFactory _roomFactory;
        
        [SerializeField] 
        private CardSet set;
        [SerializeField] private int playerHandCapacity;
        
        private bool _isGameStarted;
        private Deck _deck;
        private Dungeon _map;
        private PlayerHand _hand;
        private PlayerPawn _player;

        private Vector2Int _nextRoomPosition;
        private IRoomContent _nextRoom;

        public Deck Deck => _deck;

        public Dungeon Map => _map;

        public PlayerHand Hand => _hand;

        public PlayerPawn Player => _player;

        public event Action OnGameStarted;
        public event Action OnGameFinished;
        
        public event Action OnPlacementFailed;
        public event Action OnPlacementSuccessful;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(this);    
            }
        }

        public void StartGame()
        {
            Assert.IsTrue(!_isGameStarted);
            
            _deck = new Deck(set);
            _map = new Dungeon();
            _hand = new PlayerHand(3);
            _player = new PlayerPawn(new PlayerHealth(100));
            
            var startRoom = _map.InitRoom(Vector2Int.zero);
            _roomFactory.CreateRoom(RoomId.Empty, startRoom);
            
            Assert.IsTrue(_deck.CardCount >= _hand.HandCapacity);
            while (_hand.CanTakeCard)
            {
                var card = _deck.TakeTop();
                _hand.TakeCard(card);
            }
            
            _isGameStarted = true;
            OnGameStarted?.Invoke();
        }
        
        public void PlaceRoom(Vector2Int tilePosition, int handCardIndex)
        {
            Assert.IsTrue(_isGameStarted);
            
            var roomCard = _hand.GetCard(handCardIndex);
            bool result = _map.PlaceRoom(tilePosition, roomCard, out var room);
            if (!result)
            {
                Debug.Log("Unable to place room: " + roomCard);
                OnPlacementFailed?.Invoke();
                return;
            }
            
            _hand.PlayCard(0);

            _nextRoom = _roomFactory.CreateRoom(roomCard.RoomId, room);
            _nextRoomPosition = tilePosition;
            OnPlacementSuccessful?.Invoke();
        }

        public void MoveToNextRoom()
        {
            _player.Move(_nextRoomPosition);
        }

        public void EnterNextRoom()
        {
            _nextRoom.Enter(_player);
        }

        public void TakeCard()
        {
            if (!_hand.CanTakeCard) 
                return;
            
            if (_deck.CardCount > 0)
            {
                _hand.TakeCard(_deck.TakeTop());
            }
            else if(_hand.Cards.Count == 0)
            {
                Debug.Log("Game over!");
                OnGameFinished?.Invoke();
            }
        }
    }
}