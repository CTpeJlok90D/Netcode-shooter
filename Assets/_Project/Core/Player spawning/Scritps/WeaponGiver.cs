using UnityEngine;
using Unity.Netcode;
using Core.PlayerSpawning;
using Zenject;
using Core.Items;
using Core.Character;
using System;
using AYellowpaper.SerializedCollections;

namespace Core
{
    public class WeaponGiver : MonoBehaviour
    {
        [SerializeField] private SerializedDictionary<WeaponType, Useble> MainItems_PRFABS;
        [SerializeField] private SerializedDictionary<WeaponType, Useble> AddictionalItems_PREFABS;

        [Inject] private PlayerCharacterSpawner _playerSpawner;

        private void OnEnable()
        {
            _playerSpawner.PlayerSpawned += OnPlayerSpawn;
        }

        private void OnDisable()
        {
            _playerSpawner.PlayerSpawned -= OnPlayerSpawn;
        }

        private async void OnPlayerSpawn(SpawnConfiguration config)
        {
            try 
            {
                if (NetworkManager.Singleton.IsServer == false)
                {
                    return;
                }

                NetworkObject player;
                ulong playerOwner = config.playerID;

                if (config.player.TryGet(out player) == false) 
                {
                    return;
                }

                await Awaitable.NextFrameAsync();

                Inventory playerInventory = player.GetComponent<Inventory>();

                Useble mainWeapon = Instantiate(MainItems_PRFABS[config.mainWeapon]);
                mainWeapon.NetworkObject.SpawnWithOwnership(playerOwner);
                mainWeapon.NetworkObject.TrySetParent(player);
                

                Useble addictionalWeapon = Instantiate(AddictionalItems_PREFABS[config.addictionalWeapon]);
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
