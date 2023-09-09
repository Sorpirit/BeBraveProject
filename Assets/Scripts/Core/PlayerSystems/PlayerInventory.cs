using System;
using Core.Data.Items;
using UnityEngine;

namespace Core.PlayerSystems
{
    [Serializable]
    public class PlayerInventory
    {
        [SerializeField]
        private int gold;

        public int Gold
        {
            get => gold;
            set
            {
                int delta = value - gold;
                if(delta == 0)
                    return;
                
                if (delta > 0)
                {
                    AddGold(delta);
                }
                else
                {
                    SpendGold(delta);
                }
            }
        }
        
        public IWeapon Weapon { get; private set; }
        public IShield Shield { get; private set; }

        public Action<int> OnGoldChanged;
        public Action<IWeapon> OnWeaponChanged;
        public Action<IShield> OnShieldChanged;
        
        private void AddGold(int amount)
        {
            gold += amount;
            OnGoldChanged?.Invoke(gold);
        }
        
        private void SpendGold(int amount)
        {
            gold -= amount;
            OnGoldChanged?.Invoke(gold);
        }

        public void PickUp(IItem item)
        {
            switch (item)
            {
                case IWeapon weapon:
                    Weapon = weapon;
                    OnWeaponChanged?.Invoke(weapon);
                    break;
                case IShield shield:
                    Shield = shield;
                    OnShieldChanged?.Invoke(shield);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(item));
            }
        }
    }
}