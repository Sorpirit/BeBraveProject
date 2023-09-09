using Core.PlayerSystems;

namespace Core.Data.Items
{
    public class BasicTrap : IItem
    {
        private int _damage;
        
        public BasicTrap(int damage)
        {
            _damage = damage;
        }
        
        public void Use(PlayerPawn player)
        {
            player.HealthSystem.TakeDamage(new DamageInfo(_damage));
        }
    }
}