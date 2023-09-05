using Core.Data.Rooms;
using Core.RoomsSystem;

namespace Game.RoomFactories
{
    public interface IFactoryAggregator
    {
        void AddFactory(RoomId id, IRoomContentFactory factory);
    }
}