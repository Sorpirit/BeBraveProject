using Library.Collections;
using UnityEngine;

namespace Core.Data
{
    public readonly struct Room
    {
        public Vector2Int Position { get; }
        public NodeConnections Connections { get; }

        public Room(Vector2Int position, NodeConnections connections)
        {
            Position = position;
            Connections = connections;
        }
    }
}