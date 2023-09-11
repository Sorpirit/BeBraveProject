using UnityEngine;

namespace Game
{
    public class GridPositionConvertor : MonoBehaviour, IRoomPositionConvertor
    {
        [SerializeField] private Vector2Int gridSize;

        public Vector3 TileToWorld(Vector2Int position)
        {
            return new Vector3(position.x * gridSize.x, position.y * gridSize.y);
        }

        public Vector2Int WorldToTile(Vector3 position)
        {
            position += Vector3.one * 0.5f;
            return new Vector2Int(Mathf.FloorToInt(position.x) / gridSize.x, Mathf.FloorToInt(position.y) / gridSize.y);
        }
    }
}