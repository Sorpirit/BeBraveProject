using Core.GameStates;
using Game;
using Game.RoomFactories;
using Scripts.DependancyInjector;
using UnityEngine;
using UnityEngine.Assertions;

namespace UI
{
    public class ItemPickupController : MonoBehaviour
    {
        [Inject]
        private IPickUpCallbacks fightCallbacks;
        
        [Inject]
        private IRoomContentCallBack<ItemRoomContext> fightEncounterContextCallback;

        private GameObject _itemGO;

        private void Awake()
        {
            GameRunner.Instance.OnGameInitFinished += GameInitFinished;
        }

        private void GameInitFinished(GameContext context, GameCommander commander)
        {
            commander.PlayerEnterRoomState.OnStateEnter += ExitEmptyRoom;
        }

        private void ExitEmptyRoom()
        {
            if(_itemGO == null)
                return;

            GameRunner.Instance.Commander.PlayerEnterRoomState.Trigger();
        }

        private void Start()
        {
            fightCallbacks.OnItemPickedUp += OnItemPickedUp;
            fightEncounterContextCallback.OnRoomContentCreated += OnRoomContentCreated;
        }

        private void OnRoomContentCreated(ItemRoomContext item)
        {
            _itemGO = item.ItemGameObject;
        }

        private void OnItemPickedUp()
        {
            Destroy(_itemGO);
        }
    }
}