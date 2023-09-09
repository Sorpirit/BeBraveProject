using Core.PlayerSystems;
using UnityEngine;

namespace UI
{
    public interface IItemIconFactory
    {
        GameObject GetWeapon(IWeapon weapon);
        GameObject GetShield(IShield shield);
    }
}