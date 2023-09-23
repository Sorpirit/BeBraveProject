using System;
using Core.PlayerSystems;
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
        
        public event Action OnFightRoundFinished;
        public event Action OnEncounterFinished;
        
        public FightEncounter(IEnemy enemy)
        {
            _enemy = enemy;
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

            if (_isEnemyDead)
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
            OnEncounterFinished?.Invoke();
        }

        private void FinishFightRound()
        {
            _roundRunning = false;
            OnFightRoundFinished?.Invoke();
        }
    }
}