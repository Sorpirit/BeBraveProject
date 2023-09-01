using Library.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Core.Data
{
    public class Dungeon
    {
        private Grid<Room> _grid = new();

        public void InitRoom(Vector2Int position)
        {
            _grid.Add(position, new Room(position, NodeConnectionsExtension.AllDirections));
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
                return true;

            _grid.Remove(position);
            return false;
        }
    }
}