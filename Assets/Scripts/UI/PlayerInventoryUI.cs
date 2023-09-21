using Core.GameStates;
using Core.PlayerSystems;
using Game;
using Scripts.DependancyInjector;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerInventoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject weaponSlot;
        [SerializeField] private GameObject shieldSlot;
        
        [SerializeField] private TMP_Text goldText;
        
        [Inject]
        private IItemIconFactory _iconFactory;
        
        private GameObject _currentWeapon;
        private GameObject _currentShield;
        
        private IItemValue _weaponStats;
        private IItemValue _shieldStats;
        
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
            var inventory = GameRunner.Instance.Context.Player.Inventory;
            inventory.OnWeaponChanged += UpdateWeapon;
            inventory.OnShieldChanged += UpdateShield;
            inventory.OnGoldChanged += UpdateGold;
            
            UpdateGold(inventory.Gold);
        }

        private void UpdateGold(int gold)
        {
            goldText.text = gold.ToString();
        }

        private void UpdateShield(IShield shield)
        {   
            if(_currentShield != null)
                Destroy(_currentShield);
            
            _currentShield = Instantiate(_iconFactory.GetShield(shield), shieldSlot.transform);
            _shieldStats = _currentShield.GetComponent<IItemValue>();
            _shieldStats.SetItemValue(shield.Shield);
        }

        private void UpdateWeapon(IWeapon weapon)
        {
            if(_currentWeapon != null)
                Destroy(_currentWeapon);
            
            _currentWeapon = Instantiate(_iconFactory.GetWeapon(weapon), weaponSlot.transform);
            _weaponStats = _currentWeapon.GetComponent<IItemValue>();
            _weaponStats.SetItemValue(weapon.Damage);
        }
    }
}