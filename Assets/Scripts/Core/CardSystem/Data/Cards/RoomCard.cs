using Core.Data.Rooms;
using Library.Collections;
using UnityEngine;

namespace Core.CardSystem.Data.Cards
{
    [CreateAssetMenu(fileName = "NewRoomCard", menuName = "Core/CardSystem/CardSet")]
    public class RoomCard : CardSO
    {
        [SerializeField] private RoomId roomId;
        [SerializeField] private NodeConnections connections;

        public RoomId RoomId => roomId;

        public NodeConnections Connections => connections.ClampRange();

        public RoomCard() { }

        public RoomCard(RoomId roomId, NodeConnections connections)
        {
            this.roomId = roomId;
            this.connections = connections;
        }

        public override string ToString()
        {
            return $"{nameof(RoomId)}: {RoomId}, {nameof(Connections)}: {Connections}";
        }
    }
}