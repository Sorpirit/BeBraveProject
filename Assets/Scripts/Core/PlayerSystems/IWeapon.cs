using Core.Data.Items;

namespace Core.PlayerSystems
{
    public interface IWeapon : IItem
    {
        int Damage { get; }        
    }
}