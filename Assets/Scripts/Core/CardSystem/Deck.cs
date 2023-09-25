using System;
using System.Collections.Generic;
using Core.CardSystem.Data;
using Library.Collections;
using UnityEngine.Assertions;

namespace Core.CardSystem
{
    public class Deck
    {
        public int CardCount => _deck.Count;
        public event Action<int> OnCardCountChanged;
        
        private readonly List<ICard> _deck;
        
        public Deck(List<ICard> cards)
        {
            _deck = new List<ICard>(cards);
        }

        public void Shuffle()
        {
            _deck.Shuffle();
        }
        
        public ICard TakeTop()
        {
            Assert.IsTrue(_deck.Count > 0, "Deck is empty!");
            var result = _deck[0];
            _deck.RemoveAt(0);
            OnCardCountChanged?.Invoke(CardCount);
            return result;
        }

        public bool TryTakeTop(out ICard topPick)
        {
            topPick = default;
            if (CardCount <= 0)
                return false;

            topPick = TakeTop();
            return true;
        }

        public void PushBottom(ICard card)
        {
            _deck.Add(card);
            OnCardCountChanged?.Invoke(CardCount);
        }
    }
}