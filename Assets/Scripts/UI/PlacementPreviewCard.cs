using UnityEngine;

namespace UI
{
    public class PlacementPreviewCard : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;
            
        public void SetSprite(Sprite sprite, bool invertSprite)
        {
            spriteRenderer.sprite = sprite;
            spriteRenderer.flipX = invertSprite;
        }
    }
}