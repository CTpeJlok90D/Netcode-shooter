using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Characters;
using Core.Players;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.TextCore.Text;
using Random = UnityEngine.Random;

namespace Core.PlayerSpawning
{  
    public class PlayerCharacterSpawner : NetworkBehaviour
    {
        [SerializeField] private AssetReference _playerCharacter_PREFAB;
        [SerializeField] private Transform[] _spawnPoints;

        private Dictionary<ulong, NetworkObject> _playersObjects = new();
        private bool _gotCallback;

        public NetworkObject LocalPlayerCharacter { get; private set; }

        public delegate void PlayerSpawnedListener(NetworkObject player, ulong playerOwner);
        public event PlayerSpawnedListener PlayerSpawned;
        
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            LocalPlayerCharacter = null;
        }

        public async Task<NetworkObject> SpawnLocalPlayer()
        {
            try
            {
                if (LocalPlayerCharacter != null)
                {
                    throw new Exception("Local player already have character");
                }

                ulong localClientId = Player.Local.OwnerClientId;
                SpawnPlayer_RPC(localClientId);
                
                while (_gotCallback == false)
                {
                    await Awaitable.NextFrameAsync();
                }
                return LocalPlayerCharacter;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }
        }

        [Rpc(SendTo.Server, RequireOwnership = false)]
        private void SpawnPlayer_RPC(ulong playerID)
        {
            if (_playersObjects.ContainsKey(playerID) == false)
            {
                _playersObjects.Add(playerID, null);
            }
            
            BaseRpcTarget sender = RpcTarget.Single(playerID, RpcTargetUse.Persistent);
            NetworkObject playersObject = _playersObjects.GetValueOrDefault(playerID);
            if (playersObject == null)
            {
                Transform spawnedTransform = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
                Vector3 spawnedPosition = spawnedTransform.position;
                Addressables.InstantiateAsync(_playerCharacter_PREFAB, spawnedPosition, Quaternion.identity).Completed += (handle) => 
                {
                    NetworkObject spawnedPlayer = handle.Result.GetComponent<NetworkObject>();

                    spawnedPlayer.SpawnWithOwnership(playerID);
                    SpawnPlayerSucscessCallback_RPC(spawnedPlayer, playerID, spawnedPosition);
                };
                return;
            }
            SpawnPlayerFailedCallback_RPC(sender);
        }

        [Rpc(SendTo.Everyone)]
        private void SpawnPlayerSucscessCallback_RPC(NetworkObjectReference spawnedPlayer, ulong ownerClientID, Vector3 spawnPosition)
        {
            if (spawnedPlayer.TryGet(out NetworkObject character) == false)
            {
                return;
            }

            if (NetworkManager.LocalClientId == ownerClientID)
            {
                LocalPlayerCharacter = character;
                DelayedWarp(character, spawnPosition);
            }
            
            PlayerSpawned?.Invoke(spawnedPlayer, ownerClientID);
            _gotCallback = true;
        }

        private async void DelayedWarp(NetworkObject character, Vector3 position) 
        {
            try 
            {
                await Awaitable.NextFrameAsync();
                TopdownCharacter topdownCharacter = character.GetComponent<TopdownCharacter>();
                topdownCharacter.Warp(position);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        
        [Rpc(SendTo.SpecifiedInParams)]
        private void SpawnPlayerFailedCallback_RPC(RpcParams target)
        {
            _gotCallback = true;
        }
    }
}
