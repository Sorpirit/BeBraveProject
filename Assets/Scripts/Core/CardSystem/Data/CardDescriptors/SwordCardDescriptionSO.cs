using UnityEngine;

namespace Core.CardSystem.Data.CardDescriptors
{
    [CreateAssetMenu(menuName = "Core/CardSystem/CardDescriptors/SwordCardDescriptionSO", fileName = "NewSwordCardDescriptionSO")]
    public class SwordCardDescriptionSO : CommonCardDescriptionSO
    {
        [SerializeField] private int damageAmount;

        public int DamageAmount => damageAmount;
    }
}