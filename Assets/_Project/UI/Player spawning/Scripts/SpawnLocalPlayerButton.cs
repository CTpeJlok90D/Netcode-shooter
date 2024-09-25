using System;
using System.Threading.Tasks;
using Core.PlayerSpawning;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.PlayerSpawning
{
    public class SpawnLocalPlayerButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        [Inject] private SpawnConfigurationContainer _container;

        private PlayerCharacterSpawner _playerSpawner;

        [Inject]
        public void ZInit(PlayerCharacterSpawner playerSpawner)
        {
            _playerSpawner = playerSpawner;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
            _button.interactable = true;
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick() => _ = SpawnPlayer();
        private async Task SpawnPlayer()
        {
            try
            {
                _button.interactable = false;

                SpawnConfiguration config = _container.SpawnConfiguration;
                config.playerID = NetworkManager.Singleton.LocalClientId;
                _container.SpawnConfiguration = config;

                await _playerSpawner.SpawnLocalPlayer(_container.SpawnConfiguration);
                _button.interactable = true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                _button.interactable = true;
            }
        }
    }
}
