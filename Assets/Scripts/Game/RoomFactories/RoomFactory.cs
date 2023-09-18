using System;
using System.Collections.Generic;
using Core.Data.Rooms;
using Core.RoomsSystem;
using Core.RoomsSystem.RoomVariants;
using Game.Data;
using Library.Collections;
using Scripts.DependancyInjector;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.RoomFactories
{
    public class RoomFactory: MonoBehaviour, IRoomFactory, IFactoryAggregator
    {
        [SerializeField] private RoomOrientationSetupSO roomOrientationSetup;
        
        [Inject]
        private IRoomPositionConvertor _positionConvertor;
        
        private readonly Dictionary<RoomId, IRoomContentFactory> _factories = new();
        private Dictionary<NodeConnections, GameObject> _roomPrefabs;
        
        private void Start()
        {
            _roomPrefabs = roomOrientationSetup.RoomPrefabs;
        }

        public IRoomContent CreateRoom(RoomId id, Room room)
        {
            var position = _positionConvertor.TileToWorld(room.Position);
            var tile = Instantiate(_roomPrefabs[room.Connections], position, Quaternion.identity);

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