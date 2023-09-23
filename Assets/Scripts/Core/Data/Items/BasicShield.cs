using System;
using Core.PlayerSystems;

namespace Core.Data.Items
{
    public class BasicShield : IShield
    {
        private int _shield;

        public event Action<int> ShieldChanged;
        public int Shield => _shield;

        public BasicShield(int shield)
        {
            _shield = shield;
        }

        public void Use(PlayerPawn player)
        {
            if(_shield <= 0) 
                return;
            
            _shield--;
            ShieldChanged?.Invoke(_shield);
        }
    }
}