using Core.PlayerSystems;

namespace Core.RoomsSystem.RoomVariants
{
    public interface IEnemy
    {
        IWeapon Weapon { get; }
        IHealthSystem Health { get; }
    }
}