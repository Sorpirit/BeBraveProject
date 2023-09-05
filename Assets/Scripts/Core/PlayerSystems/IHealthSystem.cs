namespace Core.PlayerSystems
{
    public interface IHealthSystem : IDamageable
    {
        int Health { get; }

        void Heal(int amount);
    }
}