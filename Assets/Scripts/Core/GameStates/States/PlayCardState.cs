using System;
using Core.CardSystem.Data.Cards;
using Core.Data;
using Core.Data.Rooms;
using Library.GameFlow.StateSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace Core.GameStates.States
{
    public class PlayCardState : BasicMonoGameState, ICardPlacer
    {
        public IState NextState { get; set; }
        
        public event Action<Room> OnRoomPlaced; 
        public event Action<Vector2Int, int> OnPlacementFailed;
        
        public PlayCardState(GameContext context, IStateSwitcher stateSwitcher) : base(context, stateSwitcher)
        {
        }
        
        public bool PlaceRoom(Vector2Int tilePosition, int handCardIndex)
        {
            Assert.IsTrue(_isStateActive);
            Assert.IsTrue(_context.CurrentRoom.HasValue);
            var card = _context.Hand.GetCard(handCardIndex);

            switch (card)
            {
                case RoomCard roomCard:
                    var room = new Room(tilePosition, roomCard.Connections);
                    bool result = _context.Map.TryPlaceRoom(room, _context.CurrentRoom.Value);
                    if (!result)
                    {
                        Debug.Log("Unable to place room: " + roomCard);
                        OnPlacementFailed?.Invoke(tilePosition, handCardIndex);
                        return false;
                    }
                    _context.CurrentRoom = room;
                    _context.UsedRoomCard = roomCard;
                    OnRoomPlaced?.Invoke(room);
                    break;
                default:
                    throw new ArgumentException($"{card} card is unsupported for this state: {GetType()}");
            }
            
            _context.Hand.PlayCard(handCardIndex);
            _stateSwitcher.ChangeState(NextState);
            return true;
        }
    }
}