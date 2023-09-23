using Core.CardSystem;
using Core.Data;
using Core.Data.Rooms;
using Core.Data.Scriptable;
using Core.PlayerSystems;
using Core.RoomsSystem;
using Library.GameFlow.StateSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace Core.GameStates.States
{
    public class GameStartState : BasicMonoGameState
    {
        public IState NextState { get; set; }
        
        private readonly GameSetup _setup;
        
        public GameStartState(GameSetup setup, GameContext context, IStateSwitcher stateSwitcher) : base(context, stateSwitcher)
        {
            _setup = setup;
        }

        public override void EnterState()
        {
            var deck = new Deck(_setup.CardSet.Cards);
            var map = new Dungeon();
            var hand = new PlayerHand(_setup.MaxCardsInHand);
            var player = new PlayerPawn(new StandardHealthSystem(_setup.MaxPlayerHealth));

            deck.Shuffle();
            
            _context.InitContext(deck, map, hand, player, _setup.RoomFactory);
            
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
            
            _stateSwitcher.ChangeState(NextState);
        }
    }
}