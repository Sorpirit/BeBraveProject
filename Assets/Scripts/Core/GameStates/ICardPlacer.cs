using System;
using Core.Data.Rooms;
using Library.GameFlow.StateSystem;
using UnityEngine;

namespace Core.GameStates
{
    public interface ICardPlacer : IState
    {
        event Action<Room> OnRoomPlaced;
        event Action<Vector2Int, int> OnPlacementFailed;
        bool PlaceRoom(Vector2Int tilePosition, int handCardIndex);
    }
}