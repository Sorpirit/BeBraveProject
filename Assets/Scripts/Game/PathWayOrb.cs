using UnityEngine;

namespace Game
{
    public class PathWayOrb : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer renderer;
        [SerializeField] private Color connectedColor;
        
        public void Connected()
        {
            renderer.color = connectedColor;
        }
    }
}