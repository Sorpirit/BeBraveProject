using System;
using System.Collections.Generic;
using Core.Data.Rooms;
using Core.RoomsSystem;
using Core.RoomsSystem.RoomVariants;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.RoomFactories
{
    public class RoomFactory: MonoBehaviour, IRoomFactory, IFactoryAggregator
    {
        [SerializeField] private GameObject EmptyRoomPrefab;
        
        private Dictionary<RoomId, IRoomContentFactory> _factories = new();
        
        public IRoomContent CreateRoom(RoomId id, Room room)
        {
            var tile = Instantiate(EmptyRoomPrefab, new Vector3(room.Position.x, room.Position.y), Quaternion.identity);
            
            if (id == RoomId.Empty)
                return new EmptyRoomContent();

            if (_factories.TryGetValue(id, out var factory))
                return factory.CreateRoom(id, tile);
            
            throw new ArgumentException("Unknown room id");
        }

        public void AddFactory(RoomId id, IRoomContentFactory factory)
        {
            Assert.IsFalse(id == RoomId.Empty);
            Assert.IsFalse(_factories.ContainsKey(id));
            _factories.Add(id, factory);
        }
    }
}