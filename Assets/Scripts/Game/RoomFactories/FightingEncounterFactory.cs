using System;
using System.Collections.Generic;
using Core.Data.Rooms;
using Core.RoomsSystem;
using Core.RoomsSystem.RoomVariants;
using Scripts.DependancyInjector;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.RoomFactories
{
    public class FightingEncounterFactory : MonoBehaviour, IRoomContentFactory, IFightCallbacks
    {
        [SerializeField] private int basicEnemyMaxHp = 15;
        [SerializeField] private int basicEnemyDamage = 15;
        
        [SerializeField] private List<GameObject> enemyPrefabs;
        
        [Inject] 
        private IFactoryAggregator _roomFactory;
        
        public event Action OnFightRoundFinished;
        public event Action OnEncounterFinished;
        
        private IFightCallbacks _currentFight;
        
        private void Start()
        {
            _roomFactory.AddFactory(RoomId.BasicEnemy, this);
        }
        
        public IRoomContent CreateRoom(RoomId id, GameObject parentTile)
        {
            int enemyIndex = (int)id - (int) RoomId.BasicEnemy;
            var enemyGO = Instantiate(enemyPrefabs[enemyIndex], parentTile.transform);
            IRoomContent roomContent;
            switch (id)
            {
                case RoomId.BasicEnemy:
                    var encounter = new FightEncounter(new BasicEnemy(basicEnemyMaxHp, basicEnemyDamage));
                    _currentFight = encounter;
                    roomContent = encounter;
                    encounter.OnFightRoundFinished += TriggerFightRound;
                    encounter.OnEncounterFinished += TriggerFinishEncounter;
                    break;
                
                default:
                    throw new AggregateException("Factory unable to process room id: " + id);
            }

            _currentFight.OnEncounterFinished += () => Destroy(enemyGO);
            return roomContent;
        }
        
        public void TriggerRound()
        {
            Assert.IsNotNull(_currentFight);
            _currentFight.TriggerRound();
        }
        
        private void TriggerFightRound()
        {
            Assert.IsNotNull(_currentFight);
            OnFightRoundFinished?.Invoke();
        }
        
        private void TriggerFinishEncounter()
        {
            Assert.IsNotNull(_currentFight);
            OnEncounterFinished?.Invoke();
            _currentFight.OnFightRoundFinished -= TriggerFightRound;
            _currentFight.OnEncounterFinished -= TriggerFinishEncounter;
            _currentFight = null;
        }
    }
}