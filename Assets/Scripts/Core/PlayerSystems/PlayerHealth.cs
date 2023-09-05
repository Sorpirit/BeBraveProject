using UnityEngine;

namespace Core.PlayerSystems
{
    public class PlayerHealth : IHealthSystem
    {
        public int Health { get; private set; }

        public PlayerHealth(int health)
        {
            Health = health;
        }

        public void Heal(int amount)
        {
            Health += amount;
            Debug.Log($"Heal HP:{Health}(+{amount})");
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            Health -= damageInfo.DamageAmount;
            Debug.Log($"Take damage HP:{Health}(-{damageInfo.DamageAmount})");
        }
    }
}