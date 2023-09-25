using System.Collections.Generic;
using UnityEngine;

namespace Core.CardSystem.Data
{
    public abstract class CardPreset : ScriptableObject
    {
        public abstract List<ICard> ReadPreset(); 
    }
}