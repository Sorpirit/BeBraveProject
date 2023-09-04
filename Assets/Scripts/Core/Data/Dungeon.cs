using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Library.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Core.Data
{
    public class Dungeon
    {
        public event Action<Vector2Int, Room> OnRoomPlaced; 
        
        private readonly Grid<Room> _grid = new();


        public void InitRoom(Vector2Int position)
        {
            Room room = new Room(position, NodeConnectionsExtension.AllDirections);
            _grid.Add(position, room);
            OnRoomPlaced.Invoke(position, room);
        }
        
        public bool PlaceRoom(Vector2Int position, Room room)
        {
            _grid.Add(position, room);

            var neighbours = _grid.GetNeighbourValues(position, room.Connections);
            bool hasConnection = false;
            foreach (var neighbour in neighbours)
            {
                var result = NodeConnectionsExtension.TryGetNodeConnection(neighbour.Position, position,
                    out NodeConnections connection);
                Assert.IsTrue(result, $"Nodes are not adjustment: {position}, {neighbour.Position}");

                bool connected = (connection & room.Connections) != 0 && (connection.Invert() & neighbour.Connections) != 0;
                if (connected)
                {
                    _grid.Connect(position, connection);
                    hasConnection = true;
                }
            }

            if (hasConnection)
            {
                OnRoomPlaced?.Invoke(position, room);
                return true;
            }

            _grid.Remove(position);
            return false;
        }

        public ReadOnlySpan<Vector2Int> GetAvailablePlaces(Room room)
        {
            HashSet<Vector2Int> availablePositions = new HashSet<Vector2Int>();
            foreach ((Vector2Int nodePosition, NodeConnections freeConnections) in _grid.GetEdgeNodes(room.Connections))
            {
                foreach (var direction in freeConnections.GetDirections())
                {
                    availablePositions.Add(nodePosition + direction);
                }
            }

            return new ReadOnlySpan<Vector2Int>(availablePositions.ToArray());
        }
    }
}