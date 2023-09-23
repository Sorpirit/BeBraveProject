using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.CardSystem.Data
{
    [CreateAssetMenu(fileName = "NewCardSet", menuName = "Core/CardSystem/CardSet")]
    public class CardSetSO : ScriptableObject
    {
        [SerializeField] private List<CardSO> cards;

        public List<ICard> Cards => cards.Cast<ICard>().ToList();
    }
}