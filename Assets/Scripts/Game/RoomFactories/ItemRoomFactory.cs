using System;
using Core.Data.Items;
using Core.Data.Rooms;
using Core.RoomsSystem;
using Scripts.DependancyInjector;
using UnityEngine;

namespace Game.RoomFactories
{
    public class ItemRoomFactory : MonoBehaviour, IRoomFactory
    {
        [SerializeField] private GameObject coinPrefab;
        [SerializeField] private int goldAmount;
        
        [SerializeField] private GameObject potionPrefab;
        [SerializeField] private int healAmount;
        
        [Inject] 
        private IFactoryAggregator _roomFactory;
        
        private void Start()
        {
            _roomFactory.AddFactory(RoomId.Coin, this);
            _roomFactory.AddFactory(RoomId.HealthPotion, this);
        }
        
        public IRoomContent CreateRoom(RoomId id, Room room)
        {
            ItemRoomContent roomContent;
            GameObject itemGO;
            switch (id)
            {
                case RoomId.Coin:
                    itemGO = Instantiate(coinPrefab, new Vector3(room.Position.x, room.Position.y), Quaternion.identity, transform);
                    roomContent = new ItemRoomContent(new Coins(goldAmount));
                    break;
                case RoomId.HealthPotion:
                    itemGO = Instantiate(potionPrefab, new Vector3(room.Position.x, room.Position.y), Quaternion.identity, transform);
                    roomContent = new ItemRoomContent(new Coins(healAmount));
                    break;
                
                default:
                    throw new AggregateException("Factory unable to process room id: " + id);
            }

            //roomContent.OnItemUsed += () => Destroy(itemGO);
            return roomContent;
        }
    }
}