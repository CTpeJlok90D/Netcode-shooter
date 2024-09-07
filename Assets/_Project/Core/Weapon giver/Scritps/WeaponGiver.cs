using UnityEngine;
using Unity.Netcode;
using Core.PlayerSpawning;
using Zenject;
using Core.Items;
using Core.Character;
using System;

namespace Core
{
    public class WeaponGiver : NetworkBehaviour
    {
        [SerializeField] private Useble _defualtWeapon;
        [SerializeField] private Useble _defualtAddctionalWeapon;

        [Inject] private PlayerCharacterSpawner _playerSpawner;

        private void OnEnable()
        {
            _playerSpawner.PlayerSpawned += OnPlayerSpawn;
        }

        private void OnDisable()
        {
            _playerSpawner.PlayerSpawned -= OnPlayerSpawn;
        }

        private async void OnPlayerSpawn(NetworkObject player)
        {
            try 
            {
                if (NetworkManager.IsServer == false)
                {
                    return;
                }

                await Awaitable.NextFrameAsync();

                Inventory playerInventory = player.GetComponent<Inventory>();

                Useble mainWeapon = Instantiate(_defualtWeapon);
                mainWeapon.NetworkObject.SpawnWithOwnership(player.OwnerClientId);
                mainWeapon.NetworkObject.TrySetParent(player);
                playerInventory.MainWeapon.Item = mainWeapon;

                Useble addictionalWeapon = Instantiate(_defualtAddctionalWeapon);
                addictionalWeapon.NetworkObject.SpawnWithOwnership(player.OwnerClientId);
                addictionalWeapon.NetworkObject.TrySetParent(player);
                playerInventory.AddictionalWeapon.Item = addictionalWeapon;

                playerInventory.SelectWeapon(mainWeapon);
            }
            catch (Exception ex) 
            {
                Debug.LogException(ex);
            }
        }
    }
}
