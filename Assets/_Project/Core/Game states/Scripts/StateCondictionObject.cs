using System;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace Core.ConnectionStatuses
{
    public class ConnectionStatusObject : MonoBehaviour
    {
        [SerializeField] ConnectionStatus[] _acceptebleStates;
        [SerializeField] GameObject[] _targets;

        private NetworkManager NetworkManager => NetworkManager.Singleton;

        private void Start()
        {
            OnDisconnect();
            NetworkManager.OnClientStarted += OnConnect;
            NetworkManager.OnClientStopped += OnDisconnect;
            NetworkManager.OnServerStopped += OnConnect;
            NetworkManager.OnServerStopped += OnDisconnect;
        }

        private void OnDestroy()
        {
            if (NetworkManager != null)
            {
                NetworkManager.OnClientStarted -= OnConnect;
                NetworkManager.OnClientStopped -= OnDisconnect;
                NetworkManager.OnServerStopped -= OnConnect;
                NetworkManager.OnServerStopped -= OnDisconnect;
            }
        }

        private void OnConnect(bool value) => OnConnect();
        private void OnConnect()
        {
            foreach (GameObject target in _targets)
            {
               target.SetActive(_acceptebleStates.Contains(ConnectionStatus.Connected));
            }
        }

        private void OnDisconnect(bool value) => OnDisconnect();
        private void OnDisconnect()
        {
            foreach (GameObject target in _targets)
            {
                target.SetActive(_acceptebleStates.Contains(ConnectionStatus.NotConnected));
            }
        }
    }
}
