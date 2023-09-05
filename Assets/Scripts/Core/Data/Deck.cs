using System;
using System.Collections.Generic;
using Core.Data.Rooms;
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
        
        private readonly List<RoomCard> _deck;
        
        public Deck(CardSet set)
        {
            _deck = new List<RoomCard>(set.Cards);
        }

        public void Shuffle()
        {
            _deck.Shuffle();
        }
        
        public RoomCard TakeTop()
        {
            Assert.IsTrue(_deck.Count > 0, "Deck is empty!");
            var result = _deck[0];
            _deck.RemoveAt(0);
            OnCardCountChanged?.Invoke(CardCount);
            return result;
        }

        public bool TryTakeTop(out RoomCard topPick)
        {
            topPick = default;
            if (CardCount <= 0)
                return false;

            topPick = TakeTop();
            return true;
        }

        public void PushBottom(RoomCard card)
        {
            _deck.Add(card);
            OnCardCountChanged?.Invoke(CardCount);
        }
    }
}