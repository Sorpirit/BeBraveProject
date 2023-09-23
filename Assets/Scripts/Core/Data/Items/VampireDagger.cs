using System;
using Core.PlayerSystems;
using Random = UnityEngine.Random;

namespace Core.Data.Items
{
    public class VampireDagger : IWeapon
    {
        private int _damage;
        private readonly float _healthStealChance;
        private readonly int _healthStealAmount;
        public event Action<int> DamageChanged;
        
        public int Damage => _damage;

        public VampireDagger(int damage, float healthStealChance, int healthStealAmount)
        {
            _damage = damage;
            _healthStealChance = healthStealChance;
            _healthStealAmount = healthStealAmount;
        }

        public void Use(PlayerPawn player)
        {
            if(_damage <= 1) 
                return;
            
            _damage--;
            DamageChanged?.Invoke(_damage);

            if (Random.value <= _healthStealChance)
                player.HealthSystem.Heal(_healthStealAmount);
        }
    }
}