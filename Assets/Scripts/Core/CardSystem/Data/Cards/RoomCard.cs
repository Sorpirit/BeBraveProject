using Core.CardSystem.Data.CardDescriptors;
using Core.Data.Rooms;
using Library.Collections;

namespace Core.CardSystem.Data.Cards
{
    public class RoomCard : ICard
    {
        public RoomId RoomId => _roomId;
        public NodeConnections Connections => _connections.ClampRange();
        public ICardDescription Description => EmptyDescription.Instance;

        private RoomId _roomId;
        private NodeConnections _connections;
        
        public RoomCard(RoomId roomId, NodeConnections connections)
        {
            _roomId = roomId;
            _connections = connections;
        }

        public override string ToString()
        {
            return $"{nameof(RoomId)}: {RoomId}, {nameof(Connections)}: {Connections}";
        }
    }
}