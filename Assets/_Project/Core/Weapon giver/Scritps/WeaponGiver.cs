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

        private async void OnPlayerSpawn(NetworkObject player, ulong playerOwner)
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
                mainWeapon.NetworkObject.SpawnWithOwnership(playerOwner);
                mainWeapon.NetworkObject.TrySetParent(player);
                

                Useble addictionalWeapon = Instantiate(_defualtAddctionalWeapon);
                addictionalWeapon.NetworkObject.SpawnWithOwnership(playerOwner);
                addictionalWeapon.NetworkObject.TrySetParent(player);

                playerInventory.MainWeapon.Reference.SetFromServer(mainWeapon.NetworkObject);
                playerInventory.Selected.SetFromServer(mainWeapon.NetworkObject);
                playerInventory.AddictionalWeapon.Reference.SetFromServer(addictionalWeapon.NetworkObject);

            }
            catch (Exception ex) 
            {
                Debug.LogException(ex);
            }
        }
    }
}
