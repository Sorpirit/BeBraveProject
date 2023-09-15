using Core.Data.Items;
using Core.PlayerSystems;
using Core.RoomsSystem;
using System;

namespace Core.Data.Rooms
{
    public class ItemRoomContent : IRoomContent
    {
        public event Action OnItemUsed;
        
        private readonly IItem _item;

        public ItemRoomContent(IItem item)
        {
            _item = item;
        }

        public void Enter(PlayerPawn player)
        {
            _item.Use(player);
            OnItemUsed?.Invoke();
        }
    }
}