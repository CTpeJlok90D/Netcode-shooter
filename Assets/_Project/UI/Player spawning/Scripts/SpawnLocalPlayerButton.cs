using System;
using System.Threading.Tasks;
using Core.PlayerSpawning;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.PlayerSpawning
{
    public class SpawnLocalPlayerButton : MonoBehaviour
    {
        [SerializeField] Button _button;
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
                await _playerSpawner.SpawnLocalPlayer();
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
