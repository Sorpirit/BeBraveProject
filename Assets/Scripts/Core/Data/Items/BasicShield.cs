using Core.PlayerSystems;

namespace Core.Data.Items
{
    public class BasicShield : IShield
    {
        private int _shield;

        public int Shield => _shield;

        public BasicShield(int shield)
        {
            _shield = shield;
        }

        public void Use(PlayerPawn player)
        {
            _shield--;
        }
    }
}