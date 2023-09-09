using Game;
using TMPro;
using UnityEngine;

namespace UI
{
    public class BasicItemValueUI : MonoBehaviour, IItemValue
    {
        [SerializeField] private TMP_Text valueText;
        
        public void SetItemValue(int value)
        {
            valueText.text = value.ToString();
        }
    }
}