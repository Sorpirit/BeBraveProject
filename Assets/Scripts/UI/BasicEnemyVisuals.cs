using Game;
using TMPro;
using UnityEngine;

namespace UI
{
    public class BasicEnemyVisuals : MonoBehaviour, IEnemyValues
    {
        [SerializeField] private TMP_Text damageText;
        [SerializeField] private TMP_Text healthText;
        
        public void SetDamage(int value)
        {
            damageText.text = value.ToString();
        }

        public void SetHealth(int value)
        {
            healthText.text = value.ToString();
        }
    }
}