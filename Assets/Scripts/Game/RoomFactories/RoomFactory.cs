using System;
using System.Collections.Generic;
using Core.Data.Rooms;
using Core.RoomsSystem;
using Core.RoomsSystem.RoomVariants;
using Library.Collections;
using Scripts.DependancyInjector;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.RoomFactories
{
    public class RoomFactory: MonoBehaviour, IRoomFactory, IFactoryAggregator
    {
        [SerializeField] private GameObject EmptyRoomPrefab;
        [SerializeField] private PathWayOrb PathWayOrbPrefab;
        
        [Inject]
        private IRoomPositionConvertor _positionConvertor;
        
        private Dictionary<RoomId, IRoomContentFactory> _factories = new();
        
        public IRoomContent CreateRoom(RoomId id, Room room)
        {
            var position = _positionConvertor.TileToWorld(room.Position);
            var tile = Instantiate(EmptyRoomPrefab, position, Quaternion.identity);

            foreach (var direction in room.Connections.GetDirections())
            {
                var orb = Instantiate(PathWayOrbPrefab, tile.transform);
                orb.transform.localPosition = _positionConvertor.TileToWorld(direction) / 2f;
            }
            
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