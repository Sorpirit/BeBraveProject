using System.Collections.Generic;
using Core.Data;
using Core.Data.Rooms;
using Library.Collections;
using NUnit.Framework;
using UnityEngine;

namespace Tests.CoreTest.Data
{
    public class DeckTest
    {
        [Test]
        public void PickTest()
        {
            var deck = SetupBasicDeck();
            
            Assert.AreEqual(4, deck.CardCount);
            
            CheckCardTopPick(deck,RoomId.Coin);
            CheckCardTopPick(deck,RoomId.HealthPotion);
            CheckCardTopPick(deck,RoomId.BasicTrap);
            CheckCardTopPick(deck,RoomId.BasicEnemy);
            
            Assert.AreEqual(0, deck.CardCount);

            Assert.Catch(() => deck.TakeTop());
        }

        [Test]
        public void TryPickTest()
        {
            var deck = SetupBasicDeck();
            CheckCardTopPick(deck,RoomId.Coin);
            CheckCardTopPick(deck,RoomId.HealthPotion);
            
            Assert.IsTrue(deck.TryTakeTop(out var topPick) || topPick.RoomId == RoomId.BasicTrap);
            Assert.IsTrue(deck.TryTakeTop(out topPick) || topPick.RoomId == RoomId.BasicEnemy);
            Assert.IsFalse(deck.TryTakeTop(out topPick));
        }

        [Test]
        public void PushBottomTest()
        {
            var deck = SetupBasicDeck();
            Assert.AreEqual(4, deck.CardCount);
            deck.PushBottom(new RoomCard(RoomId.BasicTrap, NodeConnections.Left));
            Assert.AreEqual(5, deck.CardCount);
            deck.PushBottom(new RoomCard());

            CheckCardTopPick(deck,RoomId.Coin);
            deck.TakeTop();
            deck.TakeTop();
            deck.TakeTop();
            CheckCardTopPick(deck,RoomId.BasicTrap);
            CheckCardTopPick(deck,RoomId.Empty);
            
            Assert.AreEqual(0, deck.CardCount);
        }

        [Test]
        public void EventTest()
        {
            var deck = SetupBasicDeck();
            int count = 0;
            deck.OnCardCountChanged += i => count = i;
            
            deck.TakeTop();
            Assert.AreEqual(3, count);
            deck.PushBottom(new RoomCard(RoomId.Coin, NodeConnections.Left));
            Assert.AreEqual(4, count);
        }

        [Test]
        public void ShuffleTest()
        {
            var deckShuffled = SetupBasicDeck();
            Random.InitState(42);
            deckShuffled.Shuffle();
            
            CheckCardTopPick(deckShuffled,RoomId.BasicTrap);
            CheckCardTopPick(deckShuffled,RoomId.HealthPotion);
            CheckCardTopPick(deckShuffled,RoomId.Coin);
            CheckCardTopPick(deckShuffled,RoomId.BasicEnemy);
        }
        
        private void CheckCardTopPick(Deck deck, RoomId expected) => Assert.AreEqual(expected, deck.TakeTop().RoomId);

        private Deck SetupBasicDeck()
        {
            List<RoomCard> cards = new List<RoomCard>();
            cards.Add(new RoomCard(RoomId.Coin, NodeConnections.Down));
            cards.Add(new RoomCard(RoomId.HealthPotion, NodeConnections.Up));
            cards.Add(new RoomCard(RoomId.BasicTrap, NodeConnections.Right));
            cards.Add(new RoomCard(RoomId.BasicEnemy, NodeConnections.Left));
            return new Deck(cards);
        }
    }
}