using UnityEngine;

namespace Core.CardSystem.Data.CardDescriptors
{
    [CreateAssetMenu(menuName = "Core/CardSystem/CardDescriptors/HealingPotionCardDescriptionSO", fileName = "NewHealingPotionCardDescriptionSO")]
    public class HealingPotionCardDescriptionSO : CommonCardDescriptionSO
    {
        [SerializeField] private int healingAmount;

        public int HealingAmount => healingAmount;
    }
}