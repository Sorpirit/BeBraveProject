using System;
using System.Collections.Generic;
using Core.Data.Scriptable;
using Library.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Core.Data
{
    public class Deck
    {
        public int CardCount => _deck.Count;
        public event Action<int> OnCardCountChanged;
        
        private readonly List<Card> _deck;
        
        public Deck(CardSet set)
        {
            _deck = new List<Card>(set.Cards.Count);
            foreach (Card card in set.Cards)
            {
                
                _deck.Add(new Card(new Room(Vector2Int.zero, card.Room.Connections)));
                //Debug.Log(card);
            }
        }

        public void Shuffle()
        {
            _deck.Shuffle();
        }
        
        public Card TakeTop()
        {
            Assert.IsTrue(_deck.Count > 0, "Deck is empty!");
            var result = _deck[0];
            _deck.RemoveAt(0);
            OnCardCountChanged.Invoke(CardCount);
            return result;
        }

        public bool TryTakeTop(out Card topPick)
        {
            topPick = default;
            if (CardCount <= 0)
                return false;

            topPick = TakeTop();
            return true;
        }

        public void PushBottom(Card card)
        {
            _deck.Add(card);
            OnCardCountChanged.Invoke(CardCount);
        }
    }
}