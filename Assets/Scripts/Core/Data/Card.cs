using System;
using UnityEngine;

namespace Core.Data
{
    [Serializable]
    public struct Card
    {
        [SerializeField]
        private Room _room;

        public Room Room => _room;

        public Card(Room room)
        {
            _room = room;
        }

        public override string ToString()
        {
            return $"{nameof(_room)}: {_room}";
        }
    }
}