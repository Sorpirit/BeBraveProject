using System.Collections.Generic;
using UnityEngine;

namespace Core.CardSystem.Data
{
    [CreateAssetMenu(fileName = "NewCardSet", menuName = "Core/CardSystem/CardSet")]
    public class CardSetSO : ScriptableObject
    {
        [SerializeField] private List<CardPreset> cards;

        public List<ICard> Cards
        {
            get
            {
                var result = new List<ICard>();
                foreach (var card in cards)
                {
                    result.AddRange(card.ReadPreset());
                }

                return result;
            }
        }
    }
}