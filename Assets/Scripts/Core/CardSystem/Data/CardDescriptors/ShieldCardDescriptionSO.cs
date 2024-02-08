using UnityEngine;

namespace Core.CardSystem.Data.CardDescriptors
{
    [CreateAssetMenu(menuName = "Core/CardSystem/CardDescriptors/ShieldCardDescriptionSO", fileName = "NewShieldCardDescriptionSO")]
    public class ShieldCardDescriptionSO : CommonCardDescriptionSO
    {
        [SerializeField] private int shield;

        public int Shield => shield;
    }
}