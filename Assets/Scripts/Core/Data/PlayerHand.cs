using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Assertions;

namespace Core.Data
{
    public class PlayerHand
    {
        public event Action<ReadOnlyCollection<RoomCard>> OnHandUpdated;
        public event Action<RoomCard> OnCardAdded;
        public event Action<RoomCard> OnCardPlayed;
        public event Action<RoomCard> OnCardRemoved;
        
        public ReadOnlyCollection<RoomCard> Cards => _hand.AsReadOnly();
        public bool CanTakeCard => _hand.Count < _handCapacity;
        public int HandCapacity => _handCapacity;

        private List<RoomCard> _hand;
        private int _handCapacity;
        
        public PlayerHand(int handCapacity)
        {
            _handCapacity = handCapacity;
            _hand = new List<RoomCard>(handCapacity);
        }

        public void TakeCard(RoomCard card)
        {
            Assert.IsTrue(_hand.Count < _handCapacity, "Cant take more cards");
            _hand.Add(card);
            OnCardAdded?.Invoke(card);
            OnHandUpdated?.Invoke(Cards);
        }

        public RoomCard GetCard(int index) => _hand[index];
        
        public void PlayCard(int index)
        {
            _hand.RemoveAt(index);
            OnCardPlayed?.Invoke(_hand[index]);
            OnHandUpdated?.Invoke(Cards);
        }
        
        public void DropCard(int index)
        {
            _hand.RemoveAt(index);
            OnCardRemoved?.Invoke(_hand[index]);
            OnHandUpdated?.Invoke(Cards);
        }
    }
}