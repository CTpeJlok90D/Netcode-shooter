using Core.Conncetion;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Starter
{
    public class HostButton : MonoBehaviour
    {
        
        [SerializeField] private Button _startHostButton;

        private RelayConnection _relay;

        [Inject]
        private void ZInit(RelayConnection relayConnection)
        {
            _relay = relayConnection;
        }

        private void OnEnable()
        {
            _startHostButton.onClick.AddListener(StartHost);
        }

        private void OnDisable()
        {
            _startHostButton.onClick.RemoveListener(StartHost);
        }

        private void StartHost()
        {
            _relay.CreateRelay();
        }
    }
}
