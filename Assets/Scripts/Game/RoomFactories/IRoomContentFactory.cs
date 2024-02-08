using Core.CardSystem.Data;
using Core.RoomsSystem;
using UnityEngine;

namespace Game.RoomFactories
{
    public interface IRoomContentFactory<in T> where T : ICardDescription
    {
        IRoomContent CreateRoom(T description, GameObject tile);
    }
}