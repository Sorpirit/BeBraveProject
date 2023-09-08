using System;
using Core.Data.Rooms;
using UnityEngine;
using UnityEngine.Assertions;

namespace Core.GameStates.States
{
    public class PlayCardState : BasicMonoGameState, ICardPlacer
    {
        public event Action<Room> OnRoomPlaced; 
        public event Action<Vector2Int, int> OnPlacementFailed;
        
        public PlayCardState(GameContext context) : base(context)
        {
        }
        
        public void PlaceRoom(Vector2Int tilePosition, int handCardIndex)
        {
            Assert.IsTrue(_isStateActive);
            Assert.IsTrue(_context.CurrentRoom.HasValue);
            var roomCard = _context.Hand.GetCard(handCardIndex);
            bool result = _context.Map.PlaceRoom(tilePosition, roomCard, _context.CurrentRoom.Value, out var room);
            if (!result)
            {
                Debug.Log("Unable to place room: " + roomCard);
                OnPlacementFailed?.Invoke(tilePosition, handCardIndex);
                return;
            }

            _context.CurrentRoom = room;
            _context.UsedRoomCard = roomCard;
            OnRoomPlaced?.Invoke(room);
            _context.Hand.PlayCard(0);
            _context.ChangeState(_context.PlaceRoomState);
        }
    }
}