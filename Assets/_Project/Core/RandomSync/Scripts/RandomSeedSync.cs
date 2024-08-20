using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Custom;

namespace Core.Weapons
{
    public class RandomSeedSync : NetworkBehaviour
    {
        private NetVariable<int> _seed;

        private void Awake()
        {
            _seed = new();
        }

        private void Start()
        {
            NetworkManager.OnClientConnectedCallback += OnClientConnect;
        }

        private void OnEnable()
        {
            if (didStart)
            {
                NetworkManager.OnClientConnectedCallback += OnClientConnect;
            }
            _seed.ValueChanged += OnValueChange;
        }

        private void OnDisable()
        {
            if (NetworkManager != null)
            {
                NetworkManager.OnClientConnectedCallback -= OnClientConnect;
            }
            _seed.ValueChanged -= OnValueChange;
        }

        private void OnValueChange(int previousValue, int newValue)
        {
            Random.InitState(newValue);
        }

        private void OnClientConnect(ulong obj)
        {
            if (IsServer)
            {
                _seed.Value = Random.Range(int.MinValue, int.MaxValue);
            }
        }
    }
}
