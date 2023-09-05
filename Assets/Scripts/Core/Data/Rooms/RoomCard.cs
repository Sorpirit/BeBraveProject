using System;
using Core.Data.Rooms;
using Library.Collections;
using UnityEngine;

namespace Core.Data
{
    [Serializable]
    public class RoomCard
    {
        [SerializeField] private RoomId roomId;
        [SerializeField] private NodeConnections connections;

        public RoomId RoomId => roomId;

        public NodeConnections Connections => connections;

        public override string ToString()
        {
            return $"{nameof(RoomId)}: {RoomId}, {nameof(Connections)}: {Connections}";
        }
    }
}