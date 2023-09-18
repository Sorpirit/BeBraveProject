using System;

namespace Game.RoomFactories
{
    public interface IRoomContentCallBack<out T>
    {
        event Action<T> OnRoomContentCreated;
    }
}