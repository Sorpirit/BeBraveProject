using System;
using System.Collections.Generic;
using Core.Data.Items;
using Core.PlayerSystems;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;

namespace Core.RoomsSystem.RoomVariants
{
    public class FightEncounter : IRoomContent, IFightCallbacks
    {
        private readonly IEnemy _enemy;
        private PlayerPawn _player;
        private bool _isEnemyDead = false;
        private bool _roundRunning = false;
        
        [CanBeNull] private readonly List<IItem> _loot;
        
        public event Action OnFightRoundFinished;
        public event Action OnEncounterFinished;
        
        public FightEncounter(IEnemy enemy, List<IItem> loot = null)
        {
            _enemy = enemy;
            _loot = loot;
            _enemy.Health.OnDied += () => _isEnemyDead = true;
        }
        
        public void Enter(PlayerPawn player)
        {
            _player = player;
        }

        public void TriggerRound()
        {
            Assert.IsFalse(_roundRunning);
            Round();
        }

        private void Round()
        {
            _roundRunning = true;
            var d = new DamageInfo(_player.Inventory.Weapon.Damage);
            _enemy.Health.TakeDamage(d);
            _player.Inventory.Weapon.Use(_player);
            
            if (_isEnemyDead || _player.HealthSystem.IsDead)
            {
                FinishEncounter();
                return;
            }

            d = new DamageInfo(Mathf.Max(_enemy.Weapon.Damage - _player.Inventory.Shield.Shield, 0));
            if(d.DamageAmount > 0)
                _player.HealthSystem.TakeDamage(d);
            
            _player.Inventory.Shield.Use(_player);
            
            if (_player.HealthSystem.IsDead)
                FinishEncounter();
            else
                FinishFightRound();
        }

        private void FinishEncounter()
        {
            _roundRunning = false;
            if (_isEnemyDead && _loot != null)
            {
                foreach (var item in _loot)
                {
                    item.Use(_player);
                }
            }
            
            OnEncounterFinished?.Invoke();
        }

        private void FinishFightRound()
        {
            _roundRunning = false;
            OnFightRoundFinished?.Invoke();
        }
    }
}