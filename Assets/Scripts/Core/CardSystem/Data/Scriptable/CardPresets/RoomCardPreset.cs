using System;
using System.Collections.Generic;
using Core.CardSystem.Data.Cards;
using Core.Data.Rooms;
using Library.Collections;
using UnityEngine;

namespace Core.CardSystem.Data.Scriptable.CardPresets
{
    [CreateAssetMenu(menuName = "Core/CardSystem/CardPreset/RoomCardPreset", fileName = "NewRoomCardPreset")]
    public class RoomCardPreset : CardPreset
    {
        [SerializeField] private RoomId roomId;
        [SerializeField] private List<RoomVariant> variants;
        
        [Serializable]
        private class RoomVariant
        {
            public NodeConnections Connections = NodeConnections.None;
            public int CardCount = 1;
        }
        
        public override List<ICard> ReadPreset()
        {
            List<ICard> result = new List<ICard>(variants.Count);
            foreach (var variant in variants)
            {
                var roomCard = new RoomCard(roomId, variant.Connections);
                for (int i = 0; i < variant.CardCount; i++)
                {
                    result.Add(roomCard);
                }
            }

            return result;
        }
    }
}