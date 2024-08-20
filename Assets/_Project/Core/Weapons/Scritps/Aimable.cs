using UnityEngine;
using Unity.Netcode;
using System;
using Unity.Netcode.Custom;

namespace Core.Weapons
{
    [RequireComponent(typeof(Firearm))]
    public abstract class Aimable : NetworkBehaviour
    {
        private NetVariable<bool> _isAiming;

        public bool IsAiming 
        {
            get
            {
                return _isAiming.Value;
            }
            set
            {
                if (_isAiming.Value == value)
                {
                    return;
                }

                _isAiming.Value = value;
            }
        }

        public event Action Aimed;
        public event Action AimStopped;
        
        protected virtual void Awake()
        {
            _isAiming = new(writePerm: NetworkVariableWritePermission.Owner);
        }

        protected virtual void OnEnable()
        {
            _isAiming.ValueChanged += OnValueChange;
        }

        protected virtual void OnDisable()
        {
            _isAiming.ValueChanged -= OnValueChange;
        }

        private void OnValueChange(bool previousValue, bool newValue)
        {
            if (_isAiming.Value)
            {
                OnAim();
                Aimed?.Invoke();
            }
            else
            {
                OnAimStop();
                AimStopped?.Invoke();
            }
        }

        protected virtual void OnAim() {}
        protected virtual void OnAimStop() {}
    }
}