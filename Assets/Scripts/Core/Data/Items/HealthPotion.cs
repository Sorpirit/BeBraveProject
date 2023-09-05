using Core.PlayerSystems;

namespace Core.Data.Items
{
    public class HealthPotion : IItem
    {
        private readonly int _healAmount;

        public HealthPotion(int healAmount)
        {
            _healAmount = healAmount;
        }
        
        public void Use(PlayerPawn player)
        {
            player.HealthSystem.Heal(_healAmount);
        }
    }
}