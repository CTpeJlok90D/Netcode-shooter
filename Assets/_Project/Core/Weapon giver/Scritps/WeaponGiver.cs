using UnityEngine;
using Unity.Netcode;
using Core.PlayerSpawning;
using Zenject;
using UnityEngine.AddressableAssets;
using Core.Items;

namespace Core
{
    public class WeaponGiver : NetworkBehaviour
    {
        [SerializeField] private AssetReference _defualtWeapon;

        private PlayerCharacterSpawner _playerSpawner;

        [Inject]
        public void ZInit(PlayerCharacterSpawner playerSpawner) 
        {
            _playerSpawner = playerSpawner;
        }

        private void OnEnable()
        {
            _playerSpawner.PlayerSpawned += OnPlayerSpawn;
        }

        private void OnDisable()
        {
            _playerSpawner.PlayerSpawned -= OnPlayerSpawn;
        }

        private void OnPlayerSpawn(NetworkObject player)
        {
            if (NetworkManager.IsServer == false)
            {
                return;
            }

            UsebleReference weaponReference = player.GetComponent<UsebleReference>();
            
            Addressables.InstantiateAsync(_defualtWeapon).Completed += (handle) => 
            {
                Useble useble = handle.Result.GetComponent<Useble>();
                useble.NetworkObject.SpawnWithOwnership(player.OwnerClientId);
                useble.NetworkObject.TrySetParent(player);
                weaponReference.Value = useble;
            };
        }
    }
}
