using System;
using UnityEngine;

namespace Core.PlayerSystems
{
    public class StandardHealthSystem : IHealthSystem
    {
        private int _maxHealth;
        public event Action<int> OnHealthChanged;
        public event Action OnDied;
        
        public int Health { get; private set; }
        public int MaxHealth => _maxHealth;
        public bool IsDead { get; private set; }

        public StandardHealthSystem(int maxHealth)
        {
            _maxHealth = maxHealth;
            Health = _maxHealth;
        }

        public void Heal(int amount)
        {
            Health += amount;
            if(Health > _maxHealth)
                Health = _maxHealth;
            OnHealthChanged?.Invoke(Health);
            Debug.Log($"Heal HP:{Health}(+{amount})");
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            Health -= damageInfo.DamageAmount;
            OnHealthChanged?.Invoke(Health);
            if (Health <= 0)
            {
                OnDied?.Invoke();
                IsDead = true;
            }
            Debug.Log($"Take damage HP:{Health}(-{damageInfo.DamageAmount})");
        }
    }
}