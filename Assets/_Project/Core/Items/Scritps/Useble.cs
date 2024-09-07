using Unity.Netcode;
using Unity.Netcode.Custom;
using UnityEngine;

namespace Core.Items
{
    public class Useble : NetworkBehaviour
    {
        [field: SerializeField] public UsebleView View { get; private set; }
        [field: SerializeField] public ItemType ItemType { get; private set; }

        public GameObject Owner { get; private set; }
        public bool IsUsing { get; private set; }

        private NetVariable<NetworkObjectReference> _ownerNet;

        public delegate void UsageListener();
        public delegate void OwnerChangedListener();
        public event UsageListener UsageHasStarted;
        public event UsageListener UsageHasEnded;
        public event OwnerChangedListener OwnerChanged;

        private void Awake()
        {
            _ownerNet = new();
        }

        private void OnEnable()
        {
            _ownerNet.ValueChanged += OnValueChange;
        }

        private void OnDisable()
        {
            _ownerNet.ValueChanged -= OnValueChange;
        }

        private void Start()
        {
            Owner = _ownerNet.Value;
        }

        private void OnValueChange(NetworkObjectReference previousValue, NetworkObjectReference newValue)
        {
            Owner = newValue;
            OwnerChanged?.Invoke();
        }

        public void StartAttack()
        {
            StartAttack_RPC();
        }

        public void StopAttack()
        {
            StopAttack_RPC();
        }

        [Rpc(SendTo.Everyone)]
        private void StartAttack_RPC()
        {
            UsageHasStarted?.Invoke();
            IsUsing = true;
        }

        [Rpc(SendTo.Everyone)]
        private void StopAttack_RPC()
        {
            UsageHasEnded?.Invoke();
            IsUsing = false;
        }

        internal void SetOwner(NetworkObject newValue)
        {
            _ownerNet.Value = newValue;
        }
    }
}
