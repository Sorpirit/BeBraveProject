using System;
using Core.Data;
using Library.Collections;
using UnityEngine;

namespace Game
{
    public class RoomPlacer : MonoBehaviour
    {
        [SerializeField] private GameObject roomPrefab;
        
        private Vector2Int pointer = new(0, 1);
        private Dungeon map = new ();

        private void Start()
        {
            map.InitRoom(Vector2Int.zero);
            Instantiate(roomPrefab, new Vector3(), Quaternion.identity);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlaceRoom();
            }
        }

        private void PlaceRoom()
        {
            bool result = map.PlaceRoom(pointer, new Room(pointer, NodeConnectionsExtension.AllDirections));
            if (!result)
            {
                Debug.Log("Unable to place room");
                return;
            }

            Instantiate(roomPrefab, new Vector3(pointer.x, pointer.y), Quaternion.identity);
            pointer += Vector2Int.up;
        }
    }
}