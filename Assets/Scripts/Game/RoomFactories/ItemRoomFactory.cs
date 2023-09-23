using System;
using System.Collections.Generic;
using Core.Data.Items;
using Core.Data.Rooms;
using Core.RoomsSystem;
using Core.RoomsSystem.RoomVariants;
using Scripts.DependancyInjector;
using UnityEngine;

namespace Game.RoomFactories
{
    public class ItemRoomFactory : MonoBehaviour, IRoomContentFactory, IPickUpCallbacks, IRoomContentCallBack<ItemRoomContext>
    {
        [SerializeField] private int goldAmount;
        [SerializeField] private int healAmount;
        [SerializeField] private int trapDamageAmount;
        [SerializeField] private int basicSwordDamage;
        [SerializeField] private int basicShieldArmor;
        
        [SerializeField] private int sacrificeDaggerDamage;
        [SerializeField] private int sacrificeDaggerDamageToPlayer;
        
        [SerializeField] private int vampireDaggerDamage;
        [SerializeField] private float vampireDaggerHealChance = .4f;
        [SerializeField] private int vampireDaggerHealAmount;
        
        [SerializeField] private List<GameObject> itemsPrefabs;
        
        [Inject] 
        private IFactoryAggregator _roomFactory;
        
        public event Action OnItemPickedUp;
        public event Action<ItemRoomContext> OnRoomContentCreated;
        
        private void Start()
        {
            _roomFactory.AddFactory(RoomId.Coin, this);
            _roomFactory.AddFactory(RoomId.HealthPotion, this);
            _roomFactory.AddFactory(RoomId.SimpleSword, this);
            _roomFactory.AddFactory(RoomId.SimpleShield, this);
            _roomFactory.AddFactory(RoomId.BasicTrap, this);
            _roomFactory.AddFactory(RoomId.SacrificeDagger, this);
            _roomFactory.AddFactory(RoomId.VampireDagger, this);
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
                
                case RoomId.SacrificeDagger:
                    var sDagger = new SacrificeDagger(sacrificeDaggerDamage, sacrificeDaggerDamageToPlayer);
                    
                    itemGO.GetComponent<IItemValue>().SetItemValue(sDagger.Damage);
                    roomContent = new ItemRoomContent(new PickUpAbleItem(sDagger));
                    break;
                case RoomId.VampireDagger:
                    var vDagger = new VampireDagger(vampireDaggerDamage, vampireDaggerHealChance, vampireDaggerHealAmount);
                    
                    itemGO.GetComponent<IItemValue>().SetItemValue(vDagger.Damage);
                    roomContent = new ItemRoomContent(new PickUpAbleItem(vDagger));
                    break;
                
                default:
                    throw new AggregateException("Factory unable to process room id: " + id);
            }

            roomContent.OnItemUsed += () => OnItemPickedUp?.Invoke();
            OnRoomContentCreated?.Invoke(new ItemRoomContext(itemGO));
            return roomContent;
        }
    }

    public class ItemRoomContext
    {
        public GameObject ItemGameObject { get; }

        public ItemRoomContext(GameObject itemGameObject)
        {
            ItemGameObject = itemGameObject;
        }
    }
}