using System;
using Core.PlayerSystems;

namespace Core.Data.Items
{
    public class BasicSword : IWeapon
    {
        private int _damage;
        public event Action<int> DamageChanged;
        
        public int Damage => _damage;

        public BasicSword(int damage)
        {
            _damage = damage;
        }

        public void Use(PlayerPawn player)
        {
            if(_damage <= 1) 
                return;
            
            _damage--;
            DamageChanged?.Invoke(_damage);
        }
    }
}