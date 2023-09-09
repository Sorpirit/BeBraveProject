using Core.PlayerSystems;

namespace Core.Data.Items
{
    public class BasicSword : IWeapon
    {
        private int _damage;
        
        public int Damage => _damage;

        public BasicSword(int damage)
        {
            _damage = damage;
        }

        public void Use(PlayerPawn player)
        {
            _damage--;
        }
    }
}