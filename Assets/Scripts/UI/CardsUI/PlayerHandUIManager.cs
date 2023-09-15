using System;
using System.Collections.Generic;
using Core.Data;
using Core.GameStates;
using Game;
using JetBrains.Annotations;
using Library.Collections;
using Scripts.DependancyInjector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.CardsUI
{
    public class PlayerHandUIManager : MonoBehaviour
    {
        [SerializeField] private Transform handTransform;
        [SerializeField] private CardUI cardPrefab;
        [SerializeField] private GameObject connectionOrbPrefab;
        [SerializeField] private List<Sprite> cardSprites;
        
        [Inject]
        private IRoomPositionConvertor _positionConvertor;
        
        public int? SelectedCardIndex => _selectedCard?.Index;
        
        public event Action<int?> OnSelectionChanged;
        
        private List<CardUI> _cards = new();
        
        private CardUI _selectedCard;
        
        private void Awake()
        {
            GameRunner.Instance.OnGameContextCreated += GameContextCreated;
        }

        private void GameContextCreated(GameContext context)
        {
            context.GameStartState.OnStateEnter += GameStart;
        }

        private void GameStart()
        {
            var hand = GameRunner.Instance.Context.Hand;
            hand.OnCardAdded += AddCard;
            hand.OnCardPlayed += RemoveCard;
            hand.OnCardRemoved += RemoveCard;

            foreach (var card in hand.Cards)
            {
                AddCard(card);
            }
        }

        private void RemoveCard(RoomCard card, int index)
        {
            var cardUI = _cards[index];
            if(SelectedCardIndex == index)
                UpdateSelectedCard(null);
            
            Destroy(cardUI.gameObject);
            _cards.RemoveAt(index);
            for (int i = index; i < _cards.Count; i++)
            {
                _cards[i].UpdateCardIndex(i);
            }
        }


        public void AddCard(RoomCard card)
        {
            var cardUIGO = Instantiate(cardPrefab.gameObject, handTransform);
            var cardUI = cardUIGO.GetComponent<CardUI>();
            
            int roomIndex = (int) card.RoomId;
            var sprite = roomIndex >= 1 ? cardSprites[roomIndex - 1] : null;
            cardUI.InitCard(_cards.Count, sprite);
            cardUI.OnCardClicked += ClickedCard;
            cardUI.OnCardDrag += DragCard;
            cardUI.OnCardDrop += DropCard;
            _cards.Add(cardUI);
            
            foreach (var connection in card.Connections.GetDirections())
            {
                var orbGO = Instantiate(connectionOrbPrefab, cardUI.transform);
                var position = _positionConvertor.TileToWorld(connection);
                position.x *= 55f;
                position.y *= 75f;
                orbGO.transform.localPosition = position;
            }
        }
        
        public void DragCard(CardUI card, PointerEventData eventData)
        {
            card.transform.position = eventData.position;
        }

        public void DropCard(CardUI card, PointerEventData eventData)
        {
            card.ResetPosition(eventData);
        }

        public void ClickedCard(CardUI cardUI, PointerEventData pointerEvent)
        {
            Debug.Log("Clicked card " + cardUI.Index+" "+cardUI.name);
            UpdateSelectedCard(cardUI);
        }

        private void UpdateSelectedCard([CanBeNull] CardUI cardUI)
        {
            if (_selectedCard == cardUI)
            {
                _selectedCard.Deselect();
                _selectedCard = null;
            }
            else
            {
                _selectedCard?.Deselect();
                _selectedCard = cardUI;
                _selectedCard?.Select();
            }
            
            OnSelectionChanged?.Invoke(SelectedCardIndex);
        }
    }
}