using System;
using Core.GameStates;
using Game;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DeckUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text deckSizeText;
        
        private int _maxDeckSize;
        
        private void Awake()
        {
            GameRunner.Instance.OnGameInitFinished += GameInitFinished;
        }

        private void GameInitFinished(GameContext context, GameCommander commander)
        {
            commander.GameStartState.OnStateEnter += GameStart;
        }

        private void GameStart()
        {
            var context = GameRunner.Instance.Context;
            _maxDeckSize = context.Deck.CardCount + context.Hand.HandCapacity;
            context.Deck.OnCardCountChanged += UpdateDeckSize;
            UpdateDeckSize(context.Deck.CardCount);
        }

        private void UpdateDeckSize(int deckSize)
        {
            deckSizeText.text = deckSize + "/" + _maxDeckSize;
        }
    }
}