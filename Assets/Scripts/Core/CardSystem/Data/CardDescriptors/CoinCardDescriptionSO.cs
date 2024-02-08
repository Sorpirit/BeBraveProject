using UnityEngine;

namespace Core.CardSystem.Data.CardDescriptors
{
    [CreateAssetMenu(menuName = "Core/CardSystem/CardDescriptors/CoinCardDescriptionSO", fileName = "NewCoinCardDescriptionSO")]
    public class CoinCardDescriptionSO : CommonCardDescriptionSO
    {
        [SerializeField] private int coinAmount;

        public int CoinAmount => coinAmount;
    }
}