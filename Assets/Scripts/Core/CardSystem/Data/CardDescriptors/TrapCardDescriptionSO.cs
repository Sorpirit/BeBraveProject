using UnityEngine;

namespace Core.CardSystem.Data.CardDescriptors
{
    [CreateAssetMenu(menuName = "Core/CardSystem/CardDescriptors/TrapCardDescriptionSO", fileName = "NewTrapCardDescriptionSO")]
    public class TrapCardDescriptionSO : CommonCardDescriptionSO
    {
        [SerializeField] private int damageAmount;

        public int DamageAmount => damageAmount;
    }
}