using System;

namespace Game.RoomFactories
{
    public interface IPickUpCallbacks
    {
        event Action OnItemPickedUp; 
    }
}