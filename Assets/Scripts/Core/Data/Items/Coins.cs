using Core.PlayerSystems;

namespace Core.Data.Items
{
    public class Coins : IItem
    {
        private readonly int _goldAmount;

        public Coins(int goldAmount)
        {
            _goldAmount = goldAmount;
        }

        public void Use(PlayerPawn player)
        {
            player.Inventory.Gold += _goldAmount;
        }
    }
}