using Core.PlayerSystems;

namespace Core.Data.Items
{
    public class PickUpAbleItem : IItem
    {
        private readonly IItem _item;

        public PickUpAbleItem(IItem item)
        {
            _item = item;
        }

        public void Use(PlayerPawn player)
        {
            player.Inventory.PickUp(_item);
        }
    }
}