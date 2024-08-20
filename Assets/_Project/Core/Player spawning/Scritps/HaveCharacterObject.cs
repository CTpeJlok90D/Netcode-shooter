using Core.PlayerSpawning;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Core.Characters
{
    public class HaveCharacterObject : MonoBehaviour
    {
        [field: SerializeField] public GameObject Target { get; private set; }
        [field: SerializeField] public bool Inverse { get; private set; }
        [Inject] public PlayerCharacterSpawner PlayerCharacterSpawner { get; private set; }

        private DestroyEvent _localPlayer;

        private void OnEnable()
        {
            PlayerCharacterSpawner.PlayerSpawned += OnPlayerSpawn;
            if (didStart)
            {
                ValidateTargetActivity();
            }
        }

        private void Start()
        {
            ValidateTargetActivity();
        }

        private void OnDisable()
        {
            PlayerCharacterSpawner.PlayerSpawned -= OnPlayerSpawn;
        }

        private void OnPlayerSpawn(NetworkObject player)
        {
            if (player.TryGetComponent(out DestroyEvent localPlayer))
            {
                _localPlayer = localPlayer;
                localPlayer.Destroyed += OnLocalPlayerDestroy;
                ValidateTargetActivity();
            }
        }

        private void OnLocalPlayerDestroy(GameObject destroyedObject)
        {
            _localPlayer.Destroyed -= OnLocalPlayerDestroy;
            ValidateTargetActivity();            
        }

        private void ValidateTargetActivity()
        {
            bool active = PlayerCharacterSpawner.LocalPlayerCharacter != null && _localPlayer.IsDestroyed == false;
            if (Inverse)
            {
                active = !active;
            }
            Target.SetActive(active);
        }
    }
}