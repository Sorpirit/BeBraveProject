using System;
using Core.Data.Items;

namespace Core.PlayerSystems
{
    public interface IShield : IItem
    {
        event Action<int> ShieldChanged; 
        
        int Shield { get; }   
    }
}