using System;
using System.Collections.Generic;
using Core.CardSystem.Data;
using Core.CardSystem.Data.CardDescriptors;
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
    public class RoomFactory: MonoBehaviour, IRoomFactory, IFactoryAggregator, IRoomContentCallBack<EmptyRoomContext>
    {
        [SerializeField] private RoomOrientationSetupSO roomOrientationSetup;
        
        [Inject]
        private IRoomPositionConvertor _positionConvertor;
        
        public event Action<EmptyRoomContext> OnRoomContentCreated;
        
        private readonly Dictionary<Type, Func<ICardDescription, GameObject, IRoomContent>> _factories = new();
        private Dictionary<NodeConnections, GameObject> _roomPrefabs;
        
        private void Start()
        {
            _roomPrefabs = roomOrientationSetup.RoomPrefabs;
        }

        public IRoomContent CreateRoom(ICardDescription roomDescription, Room room)
        {
            var position = _positionConvertor.TileToWorld(room.Position);
            var tile = Instantiate(_roomPrefabs[room.Connections], position, Quaternion.identity);
            var type = roomDescription.GetType();

            if (type == typeof(EmptyDescription))
            {
                OnRoomContentCreated?.Invoke(new EmptyRoomContext());
                return new EmptyRoomContent();
            }
            
            if (_factories.TryGetValue(type, out var factoryFunction))
                return factoryFunction(roomDescription, tile);
            
            throw new ArgumentException("Unknown room id");
        }

        public void AddFactory<T>(IRoomContentFactory<T> factory) where T : ICardDescription
        {
            var type = typeof(T);
            Assert.IsFalse(type == typeof(EmptyDescription));
            Assert.IsFalse(_factories.ContainsKey(type));
            _factories.Add(type, (description, room) => factory.CreateRoom((T) description, room));
        }

        public void AddFactory<T>(Func<T, GameObject, IRoomContent> factory) where T : ICardDescription
        {
            var type = typeof(T);
            Assert.IsFalse(type == typeof(EmptyDescription));
            Assert.IsFalse(_factories.ContainsKey(type));
            _factories.Add(type, (description, room) => factory((T) description, room));
        }
    }

    public class EmptyRoomContext { }
}