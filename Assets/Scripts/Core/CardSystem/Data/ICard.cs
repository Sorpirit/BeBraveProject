using System;
using UnityEngine;

namespace Core.CardSystem.Data
{
    /// <summary>
    /// Base for all cards in the game. Every card has a description.
    /// Determent's the type of the card(aka RoomCard, ActionCard, ect.). 
    /// </summary>
    public interface ICard
    {
        public ICardDescription Description { get; }
    }
}