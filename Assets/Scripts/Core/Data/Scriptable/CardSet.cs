using System.Collections.Generic;
using UnityEngine;

namespace Core.Data.Scriptable
{
    [CreateAssetMenu(fileName = "NewCardSet", menuName = "Core/CardSet")]
    public class CardSet : ScriptableObject
    {
        [SerializeField] private List<Card> cards;

        public List<Card> Cards => cards;
    }
}