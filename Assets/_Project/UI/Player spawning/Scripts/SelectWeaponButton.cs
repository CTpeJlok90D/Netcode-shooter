using Core;
using Core.PlayerSpawning;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.PlayerSpawning
{
    public class SelectWeaponButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private WeaponType _weaponType;
        [SerializeField] private TMode _mode;

        [Inject] private SpawnConfigurationContainer _spawnConfigurationContainer;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
            _spawnConfigurationContainer.SpawnConfigurationChanged += OnSpawnConfigurationChange;
            ValidateInteracteble();
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
            _spawnConfigurationContainer.SpawnConfigurationChanged -= OnSpawnConfigurationChange;
        }

        private void OnSpawnConfigurationChange(SpawnConfiguration spawnConfiguration)
        {
            ValidateInteracteble();
        }

        private void ValidateInteracteble() 
        {
            switch (_mode)
            {
                case TMode.Main:
                    _button.interactable = _spawnConfigurationContainer.SpawnConfiguration.mainWeapon != _weaponType;
                    break;
                case TMode.Addictional:
                    _button.interactable = _spawnConfigurationContainer.SpawnConfiguration.addictionalWeapon != _weaponType;
                    break;
            }
        }

        private void OnButtonClick()
        {
            SpawnConfiguration configuration = _spawnConfigurationContainer.SpawnConfiguration;
            switch (_mode) 
            {
                case TMode.Main:
                    configuration.mainWeapon = _weaponType;
                break;
                case TMode.Addictional:
                    configuration.addictionalWeapon = _weaponType;
                    break;
            }
            _spawnConfigurationContainer.SpawnConfiguration = configuration;
        }

        private enum TMode 
        {
            Main,
            Addictional
        }
    }
}
