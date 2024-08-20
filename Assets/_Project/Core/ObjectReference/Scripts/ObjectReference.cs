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

        private NetVariable<NetworkObjectReference> _weaponReference;

        public T Value
        {
            get
            {
                return _reference;
            }
            set
            {
                _weaponReference.Value = new(value.NetworkObject);
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
            _weaponReference = new();
        }

        protected virtual void Start()
        {
            ValidateValue();
        }

        protected virtual void OnEnable()
        {
            _weaponReference.ValueChanged += OnValueChange;
        }

        protected virtual void OnDisable()
        {
            _weaponReference.ValueChanged -= OnValueChange;
        }

        private void OnValueChange(NetworkObjectReference previousValue, NetworkObjectReference newValue)
        {
            ValidateValue();
            OnValueChange();
        }

        private void ValidateValue()
        {
            if (_weaponReference.Value.TryGet(out NetworkObject networkObject))
            {
                T weapon = networkObject.GetComponent<T>();
                _reference = weapon;
                Changed?.Invoke(_reference);
            }
        }

        protected virtual void OnValueChange()
        {

        }
    }
}
