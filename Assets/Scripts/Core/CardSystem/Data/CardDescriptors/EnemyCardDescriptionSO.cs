using UnityEngine;

namespace Core.CardSystem.Data.CardDescriptors
{
    [CreateAssetMenu(menuName = "Core/CardSystem/CardDescriptors/EnemyCardDescriptionSO", fileName = "NewEnemyCardDescription")]
    public class EnemyCardDescriptionSO : CommonCardDescriptionSO
    {
        [SerializeField] private int health;
        [SerializeField] private int damage;
        
        [SerializeField] private Vector2Int coinRewardRange;
        
        public int Health => health;
        public int Damage => damage;
        
        public Vector2Int CoinRewardRange => coinRewardRange;
    }
}