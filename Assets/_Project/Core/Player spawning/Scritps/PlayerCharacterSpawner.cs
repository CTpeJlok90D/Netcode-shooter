using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Characters;
using Core.Players;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

namespace Core.PlayerSpawning
{
    public class PlayerCharacterSpawner : NetworkBehaviour
    {
        public delegate void PlayerSpawnedListener(SpawnConfiguration spawnConfiguration);


        [SerializeField] private AssetReference _playerCharacter_PREFAB;

        [SerializeField] private Transform[] _spawnPoints;

        private Dictionary<ulong, NetworkObject> _playersObjects = new();

        private bool _gotCallback;

        public NetworkObject LocalPlayerCharacter { get; private set; }

        public event PlayerSpawnedListener PlayerSpawned;

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            LocalPlayerCharacter = null;
        }

        public async Task<NetworkObject> SpawnLocalPlayer(SpawnConfiguration SpawnConfiguration)
        {
            try
            {
                if (LocalPlayerCharacter != null)
                {
                    throw new Exception("Local player already have character");
                }

                ulong localClientId = Player.Local.OwnerClientId;
                SpawnPlayer_RPC(SpawnConfiguration);
                
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
        private void SpawnPlayer_RPC(SpawnConfiguration config)
        {
            if (_playersObjects.ContainsKey(config.playerID) == false)
            {
                _playersObjects.Add(config.playerID, null);
            }
            
            BaseRpcTarget sender = RpcTarget.Single(config.playerID, RpcTargetUse.Persistent);
            NetworkObject playersObject = _playersObjects.GetValueOrDefault(config.playerID);
            if (playersObject == null)
            {
                Transform spawnedTransform = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
                config.spawnPosition = spawnedTransform.position;

                Addressables.InstantiateAsync(_playerCharacter_PREFAB, config.spawnPosition, Quaternion.identity).Completed += (handle) => 
                {
                    NetworkObject spawnedPlayer = handle.Result.GetComponent<NetworkObject>();
                    spawnedPlayer.SpawnWithOwnership(config.playerID);

                    config.player = spawnedPlayer;

                    SpawnPlayerSucscessCallback_RPC(config);
                };
                return;
            }
            SpawnPlayerFailedCallback_RPC(sender);
        }

        [Rpc(SendTo.Everyone)]
        private void SpawnPlayerSucscessCallback_RPC(SpawnConfiguration config)
        {
            if (config.player.TryGet(out NetworkObject character) == false)
            {
                return;
            }

            if (NetworkManager.LocalClientId == config.playerID)
            {
                LocalPlayerCharacter = character;
                DelayedWarp(character, config.spawnPosition);
            }

            PlayerSpawned?.Invoke(config);
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
