using Core.CardSystem.Data;
using Core.Data.Rooms;
using Library.Collections;

namespace Core.RoomsSystem
{
    public interface IRoomFactory
    {
        IRoomContent CreateRoom(ICardDescription roomDescription, Room room);
    }
}