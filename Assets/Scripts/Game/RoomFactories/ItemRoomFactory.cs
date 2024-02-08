using System;
using Core.CardSystem.Data.CardDescriptors;
using Core.Data.Items;
using Core.RoomsSystem;
using Core.RoomsSystem.RoomVariants;
using Scripts.DependancyInjector;
using UnityEngine;

namespace Game.RoomFactories
{
    public class ItemRoomFactory : 
        MonoBehaviour,
        IPickUpCallbacks, 
        IRoomContentCallBack<ItemRoomContext>
    {
        [Inject] 
        private IFactoryAggregator _roomFactory;
        
        public event Action OnItemPickedUp;
        public event Action<ItemRoomContext> OnRoomContentCreated;
        
        private void Start()
        {
            //TODO there is a bug in the item pickup. Investigate it
            _roomFactory.AddFactory<CoinCardDescriptionSO>(CreateCoinRoom);
            _roomFactory.AddFactory<HealingPotionCardDescriptionSO>(CreateHealthPotionRoom);
            _roomFactory.AddFactory<TrapCardDescriptionSO>(CreateBasicTrapRoom);
            _roomFactory.AddFactory<SwordCardDescriptionSO>(CreateSimpleSwordRoom);
            _roomFactory.AddFactory<ShieldCardDescriptionSO>(CreateSimpleShieldRoom);
            _roomFactory.AddFactory<SacrificeDaggerCardDescriptionSO>(CreateSacrificeDaggerRoom);
            _roomFactory.AddFactory<VampireDaggerCardDescriptionSO>(CreateVampireDaggerRoom);
        }
        
        private GameObject CreateCommonRoom(CommonCardDescriptionSO cardDescriptionSo, GameObject parentTile)
        {
            return Instantiate(cardDescriptionSo.Prefab, parentTile.transform);
        }

        private IRoomContent CreateItemRoomSingleValue(CommonCardDescriptionSO cardDescriptionSo, GameObject parentTile,
            IItem item, int value)
        {
            var itemGO = CreateCommonRoom(cardDescriptionSo, parentTile);
            itemGO.GetComponent<IItemValue>().SetItemValue(value);

            var roomContent = new ItemRoomContent(item);
            roomContent.OnItemUsed += () => OnItemPickedUp?.Invoke();
            OnRoomContentCreated?.Invoke(new ItemRoomContext(itemGO));
            return roomContent;
        }

        private IRoomContent CreateCoinRoom(CoinCardDescriptionSO cardDescriptionSo, GameObject parentTile) =>
            CreateItemRoomSingleValue(cardDescriptionSo, parentTile, new Coins(cardDescriptionSo.CoinAmount),
                cardDescriptionSo.CoinAmount);
        
        private IRoomContent CreateHealthPotionRoom(HealingPotionCardDescriptionSO cardDescriptionSo, GameObject parentTile) =>
            CreateItemRoomSingleValue(cardDescriptionSo, parentTile, new HealthPotion(cardDescriptionSo.HealingAmount),
                cardDescriptionSo.HealingAmount);
        
        private IRoomContent CreateBasicTrapRoom(TrapCardDescriptionSO cardDescriptionSo, GameObject parentTile) => 
            CreateItemRoomSingleValue(cardDescriptionSo, parentTile, new BasicTrap(cardDescriptionSo.DamageAmount),
                cardDescriptionSo.DamageAmount);
        
        private IRoomContent CreateSimpleSwordRoom(SwordCardDescriptionSO cardDescriptionSo, GameObject parentTile) =>
            CreateItemRoomSingleValue(cardDescriptionSo, parentTile, new PickUpAbleItem(new BasicSword(cardDescriptionSo.DamageAmount)),
                cardDescriptionSo.DamageAmount);
        
        private IRoomContent CreateSimpleShieldRoom(ShieldCardDescriptionSO cardDescriptionSo, GameObject parentTile) =>
            CreateItemRoomSingleValue(cardDescriptionSo, parentTile, new PickUpAbleItem(new BasicShield(cardDescriptionSo.Shield)),
                cardDescriptionSo.Shield);
        
        private IRoomContent CreateSacrificeDaggerRoom(SacrificeDaggerCardDescriptionSO cardDescriptionSo, GameObject parentTile) =>
            CreateItemRoomSingleValue(cardDescriptionSo, parentTile, new PickUpAbleItem(new SacrificeDagger(cardDescriptionSo.DamageAmount, cardDescriptionSo.SacrificeDaggerDamageToPlayer)),
                cardDescriptionSo.DamageAmount);

        private IRoomContent CreateVampireDaggerRoom(VampireDaggerCardDescriptionSO cardDescriptionSo, GameObject parentTile) =>
            CreateItemRoomSingleValue(cardDescriptionSo, parentTile,
                new PickUpAbleItem(new VampireDagger(cardDescriptionSo.DamageAmount, cardDescriptionSo.VampireDaggerHealChance,
                    cardDescriptionSo.VampireDaggerHealAmount)),
                cardDescriptionSo.DamageAmount);
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