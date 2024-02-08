using UnityEngine;

namespace Core.CardSystem.Data.Scriptable
{
    public abstract class CardDescriptionSO : ScriptableObject, ICardDescription
    {
        [Space]
        [Header("Basic Card Info")]
        [SerializeField] private string cardName;
        [TextArea(3, 5)]
        [SerializeField] private string cardDescription;
        
        public string CardName => cardName;
        public string CardDescription => cardDescription;
    }
}