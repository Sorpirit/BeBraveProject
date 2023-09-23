using Core.CardSystem;
using Core.CardSystem.Data.Cards;
using Core.Data;
using Core.Data.Rooms;
using Core.PlayerSystems;
using Core.RoomsSystem;
using JetBrains.Annotations;
using Library.GameFlow.StateSystem;
using UnityEngine.Assertions;

namespace Core.GameStates
{
    public class GameContext
    {
        private bool _isGameStarted;
        private bool _gameOver;
        private Deck _deck;
        private Dungeon _map;
        private PlayerHand _hand;
        private PlayerPawn _player;
        private IRoomFactory _roomFactory;

        public bool IsGameStarted => _isGameStarted;
        public Deck Deck => _deck;
        public Dungeon Map => _map;
        public PlayerHand Hand => _hand;
        public PlayerPawn Player => _player;
        public IRoomFactory RoomFactory => _roomFactory;
        
        public RoomCard? UsedRoomCard { get; set; }
        public Room? CurrentRoom { get; set; }
        [CanBeNull] public IRoomContent CurrentRoomContent { get; set; }

        public bool IsGameFinished => _player.HealthSystem.IsDead || _gameOver;
        
        public void InitContext(Deck deck, Dungeon map, PlayerHand hand, PlayerPawn player, IRoomFactory roomFactory)
        {
            _deck = deck;
            _map = map;
            _hand = hand;
            _player = player;
            _roomFactory = roomFactory;
        }
        
        public void StartGame()
        {
            _isGameStarted = true;
        }

        public void GameOver()
        {
            _gameOver = true;
        }
    }
}