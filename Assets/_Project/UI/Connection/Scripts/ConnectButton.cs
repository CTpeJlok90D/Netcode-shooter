using Core.Conncetion;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Connection
{
    public class ConnectButton : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _input;
        [SerializeField] private Button _startClientButton;
        
        private RelayConnection _relay;

        [Inject]
        private void ZInit(RelayConnection connection)
        {
            _relay = connection;
        }

        private void OnEnable()
        {
            _startClientButton.onClick.AddListener(StartClient);
        }

        private void OnDisable()
        {
            _startClientButton.onClick.RemoveListener(StartClient);
        }
        
        private void StartClient()
        {
            _relay.JoinRelay(_input.text);
        }
    }
}
