using Core.GameStates;
using DG.Tweening;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private TMP_Text healthText;
        
        [SerializeField] private float healthBarAnimationTime = 1f;
        [SerializeField] private Ease healthBarAnimationEase = Ease.OutCubic;
        
        private Tween _healthChange;
        
        private void Awake()
        {
            GameRunner.Instance.OnGameInitFinished += GameInitFinished;
        }

        private void GameInitFinished(GameContext context, GameCommander commander)
        {
            commander.GameStartState.OnStateEnter += GameStart;
        }

        private void GameStart()
        {
            var healthSystem = GameRunner.Instance.Context.Player.HealthSystem;
            healthSystem.OnHealthChanged += UpdateHealth;
            
            healthSlider.minValue = 0;
            healthSlider.maxValue = healthSystem.MaxHealth;
            
            SetHealth(healthSystem.Health);
        }
        
        private void SetHealth(int health)
        {
            _healthChange?.Kill();
            healthSlider.value = health;
            healthText.text = $"{health.ToString()}/{healthSlider.maxValue}";
        }

        private void UpdateHealth(int health)
        {
            _healthChange?.Kill();
            _healthChange = DOTween.To((v) => healthSlider.value = v, healthSlider.value, health, healthBarAnimationTime)
                .SetEase(healthBarAnimationEase);
            healthText.text = $"{health.ToString()}/{healthSlider.maxValue}";
        }
    }
}