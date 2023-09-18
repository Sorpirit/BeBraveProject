using System;
using System.Collections.Generic;
using Library.Collections;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "RoomOrientations", menuName = "Game/RoomOrientations")]
    public class RoomOrientationSetupSO : ScriptableObject
    {
        [SerializeField] private List<RoomOrientation> roomOrientations;
        
        public Dictionary<NodeConnections, GameObject> RoomPrefabs
        {
            get
            {
                var result = new Dictionary<NodeConnections, GameObject>();
                foreach (var roomOrientation in roomOrientations)
                {
                    result.Add(roomOrientation.Connections.ClampRange(), roomOrientation.Prefab);
                }

                return result;
            }
        }
        
        public Dictionary<NodeConnections, Sprite> RoomSprites
        {
            get
            {
                var result = new Dictionary<NodeConnections, Sprite>();
                foreach (var roomOrientation in roomOrientations)
                {
                    result.Add(roomOrientation.Connections.ClampRange(), roomOrientation.Sprite);
                }

                return result;
            }
        }
        
        [Serializable]
        private struct RoomOrientation
        {
            public NodeConnections Connections;
            public GameObject Prefab;
            public Sprite Sprite;
        }
    }
}