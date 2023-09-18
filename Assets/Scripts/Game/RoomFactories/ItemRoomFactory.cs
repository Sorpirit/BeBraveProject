using System;
using System.Collections.Generic;
using Core.Data.Items;
using Core.Data.Rooms;
using Core.RoomsSystem;
using Scripts.DependancyInjector;
using UnityEngine;

namespace Game.RoomFactories
{
    public class ItemRoomFactory : MonoBehaviour, IRoomContentFactory
    {
        [SerializeField] private int goldAmount;
        [SerializeField] private int healAmount;
        [SerializeField] private int trapDamageAmount;
        [SerializeField] private int basicSwordDamage;
        [SerializeField] private int basicShieldArmor;
        
        [SerializeField] private List<GameObject> itemsPrefabs;
        
        [Inject] 
        private IFactoryAggregator _roomFactory;
        
        private void Start()
        {
            _roomFactory.AddFactory(RoomId.Coin, this);
            _roomFactory.AddFactory(RoomId.HealthPotion, this);
            _roomFactory.AddFactory(RoomId.SimpleSword, this);
            _roomFactory.AddFactory(RoomId.SimpleShield, this);
            _roomFactory.AddFactory(RoomId.BasicTrap, this);
        }
        
        public IRoomContent CreateRoom(RoomId id, GameObject parentTile)
        {
            ItemRoomContent roomContent;
            GameObject itemGO;
            int index = (int) id - (int) RoomId.Coin;
            itemGO = Instantiate(itemsPrefabs[index], parentTile.transform);
            switch (id)
            {
                case RoomId.Coin:
                    itemGO.GetComponent<IItemValue>().SetItemValue(goldAmount);
                    roomContent = new ItemRoomContent(new Coins(goldAmount));
                    break;
                case RoomId.HealthPotion:
                    itemGO.GetComponent<IItemValue>().SetItemValue(healAmount);
                    roomContent = new ItemRoomContent(new HealthPotion(healAmount));
                    break;
                case RoomId.BasicTrap:
                    itemGO.GetComponent<IItemValue>().SetItemValue(trapDamageAmount);
                    roomContent = new ItemRoomContent(new BasicTrap(trapDamageAmount));
                    break;
                case RoomId.SimpleSword:
                    var sword = new BasicSword(basicSwordDamage);
                    
                    itemGO.GetComponent<IItemValue>().SetItemValue(sword.Damage);
                    roomContent = new ItemRoomContent(new PickUpAbleItem(sword));
                    break;
                case RoomId.SimpleShield:
                    var shield = new BasicShield(basicShieldArmor);
                    
                    itemGO.GetComponent<IItemValue>().SetItemValue(shield.Shield);
                    roomContent = new ItemRoomContent(new PickUpAbleItem(shield));
                    break;
                
                default:
                    throw new AggregateException("Factory unable to process room id: " + id);
            }

            roomContent.OnItemUsed += () => Destroy(itemGO);
            return roomContent;
        }
    }
}