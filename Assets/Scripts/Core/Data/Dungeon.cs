using System;
using System.Collections.Generic;
using System.Linq;
using Core.CardSystem.Data.Cards;
using Core.Data.Rooms;
using Library.Collections;
using UnityEngine;

namespace Core.Data
{
    public class Dungeon
    {
        public event Action<Vector2Int, Room> OnRoomPlaced; 
        
        private readonly Grid<Room> _grid = new();
        
        public Room InitRoom(Vector2Int position)
        {
            Room room = new Room(position, NodeConnectionsExtension.AllDirections);
            _grid.Add(position, room);
            OnRoomPlaced?.Invoke(position, room);
            return room;
        }
        
        public bool TryPlaceRoom(Room room, Room connectedRoom)
        {
            if (_grid.ContainsAt(room.Position))
                return false;
            
            bool inPossiblePositions = false;
            var possiblePositions = GetAvailablePlacesAt(connectedRoom.Position, room.Connections);
            foreach(var possiblePosition in possiblePositions)
            {
                if(possiblePosition.Equals(room.Position))
                {
                    inPossiblePositions = true;
                    break;
                }
            }
            
            if(!inPossiblePositions)
                return false;
            
            _grid.Add(room.Position, room);
            var connection = (connectedRoom.Position - room.Position).ToSingleNodeConnection();
            _grid.Connect(room.Position, connection);

            OnRoomPlaced?.Invoke(room.Position, room);
            return true;
        }
        
        public ReadOnlySpan<Vector2Int> GetAvailablePlacesAt(Vector2Int position, NodeConnections connections)
        {
            List<Vector2Int> availablePositions = new List<Vector2Int>();

            var freeConnections = _grid.GetFreeConnections(position) & _grid[position].Connections;
            var availableConnections = freeConnections & connections.Invert();
            
            if(availableConnections == NodeConnections.None)
                return ReadOnlySpan<Vector2Int>.Empty;
            
            foreach (var direction in availableConnections.GetDirections())
            {
                availablePositions.Add(position + direction);
            }

            return new ReadOnlySpan<Vector2Int>(availablePositions.ToArray());
        }
    }
}