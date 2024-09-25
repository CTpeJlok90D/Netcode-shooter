using Core.HealthSystem;
using System;
using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public class DummyRespawn : NetworkBehaviour
    {
        [SerializeField] private Health _dummy;

        [SerializeField] private float _spawnTime = 3f;

        private Health _instance;

        public override void OnNetworkSpawn()
        {
            Spawn();
        }

        private void Spawn() 
        {
            if (NetworkManager.IsServer == false) 
            {
                return;
            }

            _instance = Instantiate(_dummy, transform.position, transform.rotation);
            _instance.NetworkObject.Spawn();

            _instance.Death += OnInstanceDeath;
        }

        private async void OnInstanceDeath()
        {
            try 
            {
                await Awaitable.WaitForSecondsAsync(_spawnTime);
                Spawn();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}