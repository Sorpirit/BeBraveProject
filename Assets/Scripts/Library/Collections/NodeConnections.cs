using System;

namespace Library.Collections
{
    [Flags]
    public enum NodeConnections
    {
        None = 0,
        Up = 1,
        Right = 2,
        Down = 4,
        Left = 8,
        
        All = Up | Right | Down | Left
    }
}