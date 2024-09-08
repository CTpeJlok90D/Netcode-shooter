using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Custom;
using UnityEngine;

namespace Core
{
    public class ComponentNetworkReference<T> : NetworkBehaviour where T : NetworkBehaviour
    {
        [field: SerializeField] private T _reference;

        private List<Action> _attackedListeners = new();

        private NetVariable<NetworkObjectReference> _netReference;

        public T Value
        {
            get
            {
                return _reference;
            }
            set
            {
                _netReference.Value = new(value.NetworkObject);
            }
        }

        public delegate void ChangedListener(T newValue);
        public ChangedListener Changed;

        public event Action Used
        {
            add => _attackedListeners.Add(value);
            remove => _attackedListeners.Remove(value);
        }

        private void Awake()
        {
            _netReference = new(writePerm: NetworkVariableWritePermission.Owner);
        }

        public void Start()
        {
            ValidateValue();
        }

        protected virtual void OnEnable()
        {
            _netReference.ValueChanged += OnValueChange;
        }

        protected virtual void OnDisable()
        {
            _netReference.ValueChanged -= OnValueChange;
        }

        private void OnValueChange(NetworkObjectReference previousValue, NetworkObjectReference newValue)
        {
            ValidateValue();
            OnValueChange();
        }

        private void ValidateValue()
        {
            if (_netReference.Value.TryGet(out NetworkObject networkObject))
            {
                T component = networkObject.GetComponent<T>();
                _reference = component;
                Changed?.Invoke(_reference);
            }
        }

        protected virtual void OnValueChange()
        {

        }

        public void SetFromServer(NetworkObject reference) 
        {
            SendReference_RPC(reference);
        }

        [Rpc(SendTo.Owner, RequireOwnership = false)]
        private void SendReference_RPC(NetworkObjectReference reference) 
        {
            _netReference.Value = reference;
        }
    }
}
