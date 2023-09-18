using UnityEngine;

namespace Game.RoomFactories
{
    public class FightEncounterContext
    {
        public GameObject EnemyGameObject { get; }

        public FightEncounterContext(GameObject enemyGameObject)
        {
            EnemyGameObject = enemyGameObject;
        }
    }
}