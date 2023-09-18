using System;
using System.Collections.Generic;
using Core.Data;
using Core.GameStates;
using Game;
using Game.Data;
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
        [SerializeField] private List<Sprite> cardSprites;
        [SerializeField] private RoomOrientationSetupSO roomOrientationSetup;
        
        [Inject]
        private IRoomPositionConvertor _positionConvertor;
        
        public int? SelectedCardIndex => _selectedCard?.Index;
        
        public event Action<int?> OnSelectionChanged;
        public Func<int, Vector3, bool> TryPlace; 
        
        private List<CardUI> _cards = new();
        
        private CardUI _selectedCard;
        private Dictionary<NodeConnections, Sprite> _roomSprites;
        
        private void Awake()
        {
            GameRunner.Instance.OnGameContextCreated += GameContextCreated;
            _roomSprites = roomOrientationSetup.RoomSprites;
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
            bool invertSprite = card.Connections.HasFlag(NodeConnections.Right) && !card.Connections.HasFlag(NodeConnections.Left);
            cardUI.InitCard(_cards.Count, _roomSprites[card.Connections], invertSprite, sprite);
            cardUI.OnCardClicked += ClickedCard;
            cardUI.OnCardDrag += DragCard;
            cardUI.OnCardDrop += DropCard;
            _cards.Add(cardUI);
        }
        
        public void DragCard(CardUI cardUI, PointerEventData eventData)
        {
            cardUI.transform.position = eventData.position;
            
            if (_selectedCard != cardUI)
                UpdateSelectedCard(cardUI);
        }

        public void DropCard(CardUI cardUI, PointerEventData eventData)
        {
            if(!TryPlace?.Invoke(cardUI.Index, cardUI.transform.position) ?? false)
                cardUI.ResetPosition(eventData);
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