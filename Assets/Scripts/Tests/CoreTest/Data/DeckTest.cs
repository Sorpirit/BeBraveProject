using System.Collections.Generic;
using Core.CardSystem;
using Core.CardSystem.Data;
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
            
            Assert.That(deck.CardCount, Is.EqualTo(4));
            
            CheckCardTopPick(deck,0);
            CheckCardTopPick(deck,1);
            CheckCardTopPick(deck,2);
            CheckCardTopPick(deck,3);
            
            Assert.That(deck.CardCount, Is.EqualTo(0));

            Assert.Catch(() => deck.TakeTop());
        }

        [Test]
        public void TryPickTest()
        {
            var deck = SetupBasicDeck();
            CheckCardTopPick(deck,0);
            CheckCardTopPick(deck,1);
            
            Assert.IsTrue(deck.TryTakeTop(out var topPick) || ((TestCard)topPick).ID == 2);
            Assert.IsTrue(deck.TryTakeTop(out topPick) || ((TestCard)topPick).ID == 3);
            Assert.IsFalse(deck.TryTakeTop(out topPick));
        }

        [Test]
        public void PushBottomTest()
        {
            var deck = SetupBasicDeck();
            Assert.AreEqual(4, deck.CardCount);
            deck.PushBottom(new TestCard(42));
            Assert.AreEqual(5, deck.CardCount);
            deck.PushBottom(new TestCard(0));

            CheckCardTopPick(deck,0);
            deck.TakeTop();
            deck.TakeTop();
            deck.TakeTop();
            CheckCardTopPick(deck,42);
            CheckCardTopPick(deck, 0);
            
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
            deck.PushBottom(new TestCard(42));
            Assert.AreEqual(4, count);
        }

        [Test]
        public void ShuffleTest()
        {
            var deckShuffled = SetupBasicDeck();
            Random.InitState(42);
            deckShuffled.Shuffle();
            
            CheckCardTopPick(deckShuffled, 2);
            CheckCardTopPick(deckShuffled, 1);
            CheckCardTopPick(deckShuffled, 0);
            CheckCardTopPick(deckShuffled, 3);
        }
        
        private void CheckCardTopPick(Deck deck, int testCardId) => Assert.That((deck.TakeTop() as TestCard)?.ID, Is.EqualTo(testCardId));

        private Deck SetupBasicDeck()
        {
            List<ICard> cards = new List<ICard>();
            cards.Add(new TestCard(0));
            cards.Add(new TestCard(1));
            cards.Add(new TestCard(2));
            cards.Add(new TestCard(3));
            return new Deck(cards);
        }
        
        private class TestCard : ICard
        {
            public int ID { get; }
            public ICardDescription Description { get; }

            public TestCard(int id)
            {
                ID = id;
            }

            
        }
    }
}