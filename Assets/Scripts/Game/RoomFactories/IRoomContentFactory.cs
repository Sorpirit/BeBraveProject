using Core.Data.Rooms;
using Core.RoomsSystem;
using UnityEngine;

namespace Game.RoomFactories
{
    public interface IRoomContentFactory
    {
        IRoomContent CreateRoom(RoomId id, GameObject tile);
    }
}