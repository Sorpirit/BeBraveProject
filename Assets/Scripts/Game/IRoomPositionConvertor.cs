using UnityEngine;

namespace Game
{
    public interface IRoomPositionConvertor
    {
        Vector3 TileToWorld(Vector2Int position);
        Vector2Int WorldToTile(Vector3 position);
    }
}