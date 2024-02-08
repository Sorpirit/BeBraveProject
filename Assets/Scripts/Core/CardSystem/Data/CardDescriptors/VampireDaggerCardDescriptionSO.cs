using UnityEngine;

namespace Core.CardSystem.Data.CardDescriptors
{
    [CreateAssetMenu(menuName = "Core/CardSystem/CardDescriptors/VampireDaggerCardDescriptionSO", fileName = "NewVampireDaggerCardDescriptionSO")]
    public class VampireDaggerCardDescriptionSO : SwordCardDescriptionSO
    {
        [SerializeField] private float vampireDaggerHealChance = .4f;
        [SerializeField] private int vampireDaggerHealAmount;

        public float VampireDaggerHealChance => vampireDaggerHealChance;
        public int VampireDaggerHealAmount => vampireDaggerHealAmount;
    }
}