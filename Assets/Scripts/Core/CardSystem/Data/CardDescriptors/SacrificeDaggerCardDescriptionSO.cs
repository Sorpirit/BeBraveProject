using UnityEngine;

namespace Core.CardSystem.Data.CardDescriptors
{
    [CreateAssetMenu(menuName = "Core/CardSystem/CardDescriptors/SacrificeDaggerCardDescriptionSO", fileName = "NewSacrificeDaggerCardDescriptionSO")]
    public class SacrificeDaggerCardDescriptionSO : SwordCardDescriptionSO
    {
        [SerializeField] private int sacrificeDaggerDamageToPlayer;

        public int SacrificeDaggerDamageToPlayer => sacrificeDaggerDamageToPlayer;
    }
}