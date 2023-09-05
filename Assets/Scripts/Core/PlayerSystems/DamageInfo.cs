namespace Core.PlayerSystems
{
    public readonly struct DamageInfo
    {
        public int DamageAmount { get; }

        public DamageInfo(int damageAmount)
        {
            DamageAmount = damageAmount;
        }
    }
}