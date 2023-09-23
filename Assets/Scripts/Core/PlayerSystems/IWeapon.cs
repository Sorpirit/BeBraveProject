using System;
using Core.Data.Items;

namespace Core.PlayerSystems
{
    public interface IWeapon : IItem
    {
        event Action<int> DamageChanged;
        
        int Damage { get; }        
    }
}