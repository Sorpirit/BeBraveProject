using System;
using Library.Collections;
using UnityEngine;

namespace Core.Data.Rooms
{
    [Serializable]
    public struct Room
    {
        [SerializeField]
        private Vector2Int _position;
        [SerializeField]
        private NodeConnections _connections;

        public Vector2Int Position => _position;

        public NodeConnections Connections => _connections.ClampRange();

        public Room(Vector2Int position, NodeConnections connections)
        {
            _position = position;
            _connections = connections;
        }

        public override string ToString()
        {
            return $"{nameof(Position)}: {Position}, {nameof(Connections)}: {Connections}";
        }
    }
}