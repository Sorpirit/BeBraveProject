using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

namespace UI.CardsUI
{
    public class CardUI : MonoBehaviour, IPointerClickHandler, IDragHandler, IDropHandler, IBeginDragHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private Image iconImage;
        [SerializeField] private Color selectedColor;
        [SerializeField] private Color unselectedColor;

        public int Index { get; private set; }
        
        public event Action<CardUI, PointerEventData> OnCardClicked;
        public event Action<CardUI, PointerEventData> OnCardDrag;
        public event Action<CardUI, PointerEventData> OnCardDrop;
        
        private Vector3 _originalPosition;

        public void ResetPosition(PointerEventData pointerEventData)
        {
            transform.position = _originalPosition;
        }

        public void Deselect()
        {
            image.color = unselectedColor;
        }

        public void Select()
        {
            image.color = selectedColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnCardClicked?.Invoke(this, eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnCardDrag?.Invoke(this, eventData);
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnCardDrop?.Invoke(this, eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalPosition = transform.position;
        }

        public void InitCard(int cardIndex, [CanBeNull] Sprite sprite)
        {
            Index = cardIndex;
            if(sprite != null)
            {
                iconImage.sprite = sprite;
                iconImage.color = Color.black;
            }
        }
        
        public void UpdateCardIndex(int cardIndex)
        {
            Index = cardIndex;
        }
    }
}