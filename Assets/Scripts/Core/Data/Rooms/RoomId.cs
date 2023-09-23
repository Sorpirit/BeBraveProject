using System;

namespace Core.Data.Rooms
{
    [Serializable]
    public enum RoomId
    {
        Empty,
        
        //Item pick up
        Coin,
        HealthPotion,
        SimpleSword,
        SacrificeDagger,
        VampireDagger,
        SimpleShield,

        //Trap
        BasicTrap,
        
        //Fighting encounter
        BasicEnemy,
    }
}