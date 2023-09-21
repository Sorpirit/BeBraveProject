using System;
using Core.GameStates;
using Game;
using Game.RoomFactories;
using Scripts.DependancyInjector;
using UnityEngine;
using UnityEngine.Assertions;

namespace UI
{
    public class EmptyRoomController : MonoBehaviour
    {
        [Inject]
        private IRoomContentCallBack<EmptyRoomContext> fightEncounterContextCallback;

        private bool _emptyRoom;

        private void Awake()
        {
            GameRunner.Instance.OnGameInitFinished += GameInitFinished;
        }

        private void GameInitFinished(GameContext context, GameCommander commander)
        {
            commander.PlayerEnterRoomState.OnStateEnter += ExitEmptyRoom;
        }

        private void Start()
        {
            fightEncounterContextCallback.OnRoomContentCreated += OnRoomContentCreated;
        }

        private void OnRoomContentCreated(EmptyRoomContext item)
        {
            _emptyRoom = true;
        }

        private void ExitEmptyRoom()
        {
            if(!_emptyRoom)
                return;

            _emptyRoom = false;
            GameRunner.Instance.Commander.PlayerEnterRoomState.Trigger();
        }
    }
}