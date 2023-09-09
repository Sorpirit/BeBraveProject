using Core.Data;
using Core.Data.Rooms;
using Core.Data.Scriptable;
using Core.PlayerSystems;
using Core.RoomsSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace Core.GameStates.States
{
    public class GameStartState : BasicMonoGameState
    {
        private readonly CardSet _set;
        private readonly IRoomFactory _roomFactory;

        public GameStartState(GameContext context, CardSet set, IRoomFactory roomFactory) : base(context)
        {
            _set = set;
            _roomFactory = roomFactory;
        }

        public override void EnterState()
        {
            var deck = new Deck(_set);
            var map = new Dungeon();
            var hand = new PlayerHand(3);
            var player = new PlayerPawn(new PlayerHealth(32));

            _context.InitContext(deck, map, hand, player, _roomFactory);
            
            var startRoom = _context.Map.InitRoom(Vector2Int.zero);
            var startContentRoom = _context.RoomFactory.CreateRoom(RoomId.Empty, startRoom);
            _context.CurrentRoom = startRoom;
            _context.CurrentRoomContent = startContentRoom;
            
            Assert.IsTrue(_context.Deck.CardCount >= _context.Hand.HandCapacity);
            while (_context.Hand.CanTakeCard)
            {
                var card = _context.Deck.TakeTop();
                _context.Hand.TakeCard(card);
            }
            
            _context.StartGame();
            base.EnterState();
            
            _context.ChangeState(_context.PlayCardState);
        }
    }
}