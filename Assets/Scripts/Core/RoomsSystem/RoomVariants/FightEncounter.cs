using System;
using Core.PlayerSystems;

namespace Core.RoomsSystem.RoomVariants
{
    public class FightEncounter : IRoomContent
    {
        private readonly IEnemy _enemy;
        private bool _isEnemyDead = false;

        public event Action OnFightRoundFinished;
        public event Action OnEncounterFinished;
        
        public FightEncounter(IEnemy enemy)
        {
            _enemy = enemy;
            _enemy.Health.OnDied += () => _isEnemyDead = true;
        }
        
        public void Enter(PlayerPawn player)
        {
            var d = new DamageInfo(player.Inventory.Weapon.Damage);
            _enemy.Health.TakeDamage(d);
            player.Inventory.Weapon.Use(player);

            if (_isEnemyDead)
                FinishFightRound();

        }

        private void FinishFightRound()
        {
            OnEncounterFinished?.Invoke();
        }
    }
}