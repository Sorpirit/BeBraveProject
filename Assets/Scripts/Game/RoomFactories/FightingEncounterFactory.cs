using System;
using System.Collections.Generic;
using Core.CardSystem.Data;
using Core.CardSystem.Data.CardDescriptors;
using Core.Data.Items;
using Core.RoomsSystem;
using Core.RoomsSystem.RoomVariants;
using Scripts.DependancyInjector;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace Game.RoomFactories
{
    public class FightingEncounterFactory : MonoBehaviour, IRoomContentFactory<EnemyCardDescriptionSO>, IFightCallbacks, IRoomContentCallBack<FightEncounterContext>
    {
        [Inject] 
        private IFactoryAggregator _roomFactory;
        
        public event Action OnFightRoundFinished;
        public event Action OnEncounterFinished;
        public event Action<FightEncounterContext> OnRoomContentCreated;
        
        private IFightCallbacks _currentFight;
        
        private void Start()
        {
            _roomFactory.AddFactory(this);
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

        public IRoomContent CreateRoom(EnemyCardDescriptionSO description, GameObject parentTile)
        {
            var enemyGO = Instantiate(description.Prefab, parentTile.transform);
            var enemyValues = enemyGO.GetComponent<IEnemyValues>();
                    
            var enemy = new BasicEnemy(description.Health, description.Damage);
            
            int coinReward = Random.Range(description.CoinRewardRange.x, description.CoinRewardRange.y);
            List<IItem> loot = new List<IItem> {new Coins(coinReward)};
            var encounter = new FightEncounter(enemy, loot);
            
            _currentFight = encounter;
            encounter.OnFightRoundFinished += TriggerFightRound;
            encounter.OnEncounterFinished += TriggerFinishEncounter;
                    
            enemyValues.
                SetDamage(enemy.Weapon.Damage);
            enemyValues.SetHealth(enemy.Health.Health);
                    
            enemy.Weapon.DamageChanged += enemyValues.SetDamage;
            enemy.Health.OnHealthChanged += enemyValues.SetHealth;
            OnRoomContentCreated?.Invoke(new FightEncounterContext(enemyGO));
            return encounter;
        }
    }
}