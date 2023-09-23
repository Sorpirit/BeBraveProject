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
    public class FightingEncounterFactory : MonoBehaviour, IRoomContentFactory, IFightCallbacks, IRoomContentCallBack<FightEncounterContext>
    {
        [SerializeField] private int basicEnemyMaxHp = 15;
        [SerializeField] private int basicEnemyDamage = 15;
        
        [SerializeField] private List<GameObject> enemyPrefabs;
        
        [Inject] 
        private IFactoryAggregator _roomFactory;
        
        public event Action OnFightRoundFinished;
        public event Action OnEncounterFinished;
        public event Action<FightEncounterContext> OnRoomContentCreated;
        
        private IFightCallbacks _currentFight;
        
        private void Start()
        {
            _roomFactory.AddFactory(RoomId.BasicEnemy, this);
        }
        
        public IRoomContent CreateRoom(RoomId id, GameObject parentTile)
        {
            int enemyIndex = (int)id - (int) RoomId.BasicEnemy;
            var enemyGO = Instantiate(enemyPrefabs[enemyIndex], parentTile.transform);
            var enemyValues = enemyGO.GetComponent<IEnemyValues>();
            IRoomContent roomContent;
            switch (id)
            {
                case RoomId.BasicEnemy:
                    var enemy = new BasicEnemy(basicEnemyMaxHp, basicEnemyDamage);
                    var encounter = new FightEncounter(enemy);
                    _currentFight = encounter;
                    roomContent = encounter;
                    encounter.OnFightRoundFinished += TriggerFightRound;
                    encounter.OnEncounterFinished += TriggerFinishEncounter;
                    
                    enemyValues.
                        SetDamage(enemy.Weapon.Damage);
                    enemyValues.SetHealth(enemy.Health.Health);
                    
                    enemy.Weapon.DamageChanged += enemyValues.SetDamage;
                    enemy.Health.OnHealthChanged += enemyValues.SetHealth;
                    
                    break;
                
                default:
                    throw new AggregateException("Factory unable to process room id: " + id);
            }

            OnRoomContentCreated?.Invoke(new FightEncounterContext(enemyGO));
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