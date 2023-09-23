using System;
using Core.PlayerSystems;

namespace Core.Data.Items
{
    public class SacrificeDagger : IWeapon
    {
        private int _damage;
        private readonly int _playerDamage;
        public event Action<int> DamageChanged;
        
        public int Damage => _damage;

        public SacrificeDagger(int damage, int playerDamage)
        {
            _damage = damage;
            _playerDamage = playerDamage;
        }

        public void Use(PlayerPawn player)
        {
            player.HealthSystem.TakeDamage(new DamageInfo(_playerDamage));
        }
    }
}