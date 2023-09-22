using System;
using Core.Data.Items;
using Core.PlayerSystems;

namespace Core.RoomsSystem.RoomVariants
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