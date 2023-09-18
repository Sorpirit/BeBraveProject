using Core.Data.Items;
using Core.PlayerSystems;

namespace Core.RoomsSystem.RoomVariants
{
    public class BasicEnemy : IEnemy
    {
        public IWeapon Weapon { get; }
        public IHealthSystem Health { get; }
        
        public BasicEnemy(int maxHp, int damage)
        {
            Health = new StandardHealthSystem(maxHp);
            Weapon = new BasicSword(damage);
        }
    }
}