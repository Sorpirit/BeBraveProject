using Core.PlayerSystems;
using UnityEngine;

namespace Core.RoomsSystem.RoomVariants
{
    public class EmptyRoomContent : IRoomContent
    {
        public void Enter(PlayerPawn player)
        {
            Debug.Log("Enter empty room");
        }
    }
}