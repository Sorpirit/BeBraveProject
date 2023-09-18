using System;

namespace Core.PlayerSystems
{
    public interface IHealthSystem : IDamageable
    {
        event Action<int> OnHealthChanged;
        event Action OnDied;
        
        int Health { get; }
        int MaxHealth { get; }
        bool IsDead { get; }

        void Heal(int amount);
    }
}