using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Core.CardSystem.Data;
using Core.Data;
using UnityEngine.Assertions;

namespace Core.PlayerSystems
{
    public class PlayerHand
    {
        public event Action<ReadOnlyCollection<ICard>> OnHandUpdated;
        public event Action<ICard> OnCardAdded;
        public event Action<ICard, int> OnCardPlayed;
        public event Action<ICard, int> OnCardRemoved;
        
        public ReadOnlyCollection<ICard> Cards => _hand.AsReadOnly();
        public bool CanTakeCard => _hand.Count < _handCapacity;
        public int HandCapacity => _handCapacity;

        private List<ICard> _hand;
        private int _handCapacity;
        
        public PlayerHand(int handCapacity)
        {
            _handCapacity = handCapacity;
            _hand = new List<ICard>(handCapacity);
        }

        public void TakeCard(ICard card)
        {
            Assert.IsTrue(_hand.Count < _handCapacity, "Cant take more cards");
            _hand.Add(card);
            OnCardAdded?.Invoke(card);
            OnHandUpdated?.Invoke(Cards);
        }

        public ICard GetCard(int index) => _hand[index];
        
        public void PlayCard(int index)
        {
            OnCardPlayed?.Invoke(_hand[index], index);
            _hand.RemoveAt(index);
            OnHandUpdated?.Invoke(Cards);
        }
        
        public void DropCard(int index)
        {
            OnCardRemoved?.Invoke(_hand[index], index);
            _hand.RemoveAt(index);
            OnHandUpdated?.Invoke(Cards);
        }
    }
}