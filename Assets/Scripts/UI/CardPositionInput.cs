using System;
using Game;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class CardPositionInput : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private GameObject coursewareVisualiser;
        
        private void Update()
        {
            var mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition += Vector3.one * 0.5f;
            var tilePosition = new Vector2Int(Mathf.FloorToInt(mouseWorldPosition.x),
                Mathf.FloorToInt(mouseWorldPosition.y));

            coursewareVisualiser.transform.position = new Vector3(tilePosition.x, tilePosition.y);
            
            if (Input.GetMouseButtonDown(0))
            {
                
                GameMaster.Instance.PlaceRoom(tilePosition, 0);
            }
        }
    }
}