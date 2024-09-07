using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Players;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;
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
        public delegate void PlayerSpawnedListener(NetworkObject player);
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
                Addressables.InstantiateAsync(_playerCharacter_PREFAB).Completed += (handle) => 
                {
                    NetworkObject spawnedPlayer = handle.Result.GetComponent<NetworkObject>();

                    Transform spawnedTransform = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
                    Vector3 spawnedPosition = spawnedTransform.position;
                    spawnedPlayer.transform.position = spawnedPosition;
                    spawnedPlayer.SpawnWithOwnership(playerID);

                    SpawnPlayerSucscessCallback_RPC(spawnedPlayer);
                };
                return;
            }
            SpawnPlayerFailedCallback_RPC(sender);
        }

        [Rpc(SendTo.Everyone)]
        private void SpawnPlayerSucscessCallback_RPC(NetworkObjectReference spawnedPlayer)
        {
            if (spawnedPlayer.TryGet(out NetworkObject character) == false)
            {
                return;
            }

            if (character.IsOwner)
            {
                LocalPlayerCharacter = character;
            }
            
            PlayerSpawned?.Invoke(spawnedPlayer);
            _gotCallback = true;
        }

        
        [Rpc(SendTo.SpecifiedInParams)]
        private void SpawnPlayerFailedCallback_RPC(RpcParams target)
        {
            _gotCallback = true;
        }
    }
}
