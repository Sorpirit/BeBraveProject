using Core.Data.Rooms;
using Library.Collections;

namespace Core.RoomsSystem
{
    public interface IRoomFactory
    {
        IRoomContent CreateRoom(RoomId id, Room room);
    }
}