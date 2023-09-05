using System;
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

        private void AddGold(int amount)
        {
            gold += amount;
        }
        
        private void SpendGold(int amount)
        {
            gold -= amount;
        }
    }
}