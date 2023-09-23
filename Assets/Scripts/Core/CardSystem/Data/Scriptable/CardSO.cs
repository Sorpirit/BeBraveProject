using Core.CardSystem.Data.Scriptable;
using UnityEngine;

namespace Core.CardSystem.Data
{
    public abstract class CardSO : ScriptableObject, ICard
    {
        [SerializeField] private CardDescriptionSO description;

        public ICardDescription Description => description;
    }
}