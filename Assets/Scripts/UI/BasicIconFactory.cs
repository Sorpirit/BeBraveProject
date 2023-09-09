using System;
using Core.Data.Items;
using Core.PlayerSystems;
using UnityEngine;

namespace UI
{
    public class BasicIconFactory : MonoBehaviour, IItemIconFactory
    {
        [SerializeField] private GameObject basicSwordPrefab;
        [SerializeField] private GameObject basicShieldPrefab;
        
        public GameObject GetWeapon(IWeapon weapon)
        {
            switch (weapon)
            {
                case BasicSword:
                    return basicSwordPrefab;
                
                default:
                    throw new ArgumentOutOfRangeException("Inappropriate weapon type");
            }
        }

        public GameObject GetShield(IShield shield)
        {
            switch (shield)
            {
                case BasicShield:
                    return basicShieldPrefab;
                
                default:
                    throw new ArgumentOutOfRangeException("Inappropriate shield type");
            }
        }
    }
}