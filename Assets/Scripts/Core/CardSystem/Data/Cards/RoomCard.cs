using Core.CardSystem.Data.CardDescriptors;
using Core.Data.Rooms;
using Library.Collections;

namespace Core.CardSystem.Data.Cards
{
    public class RoomCard : ICard
    {
        public RoomId RoomId => _roomId;
        public NodeConnections Connections => _connections.ClampRange();
        public ICardDescription Description => _description;

        private RoomId _roomId;
        private NodeConnections _connections;
        private ICardDescription _description;
        
        public RoomCard(RoomId roomId, NodeConnections connections, ICardDescription description = null)
        {
            _roomId = roomId;
            _connections = connections;
            _description = description ?? EmptyDescription.Instance;
        }

        public override string ToString()
        {
            return $"{nameof(RoomId)}: {RoomId}, {nameof(Connections)}: {Connections}";
        }
    }
}