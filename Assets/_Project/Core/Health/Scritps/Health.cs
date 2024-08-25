using System;
using Unity.Netcode;
using Unity.Netcode.Custom;
using UnityEditor;
using UnityEngine;

namespace Core.HealthSystem
{
    public class Health : NetworkBehaviour
    {
        [field: SerializeField] public float Max { get; private set; } = 100;
        private NetVariable<float> _value;
        public DamageInfo LastDamageInfo { get; private set; }

        public float Value
        {
            get
            {
                return _value.Value;
            }
            private set
            {
                _value.Value = Mathf.Clamp(value, 0, Max);
            }
        }

        public delegate void DeathListener();
        public event DeathListener Death;
        public event NetworkVariable<float>.OnValueChangedDelegate Changed;

        private void Awake()
        {
            _value = new NetVariable<float>(100);   
        }

        public void DealDamage(DamageInfo info)
        {
            LastDamageInfo = info;
            if (NetworkManager.IsServer == false)
            {
                return;
            }
            
            _value.Value = Mathf.Clamp(_value.Value - info.Count, 0, Max);
            if (_value.Value == 0)
            {
                Kill();
            }
        }

        private void OnEnable()
        {
            _value.ValueChanged += OnValueChange;
        }

        private void OnDisable()
        {
            _value.ValueChanged -= OnValueChange;
        }

        private void OnValueChange(float previousValue, float newValue)
        {
            Changed?.Invoke(previousValue, _value.Value);
        }

        public void Kill()
        {
            float oldValue = _value.Value;
            _value.Value = 0;
            Changed?.Invoke(oldValue, _value.Value);
            
            Death_RPC();
            NetworkObject.Despawn(true);
        }

        [Rpc(SendTo.Everyone)]
        private void Death_RPC()
        {
            Death?.Invoke();
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(Health))]
        private class CEditor : Editor
        {
            private Health Health => target as Health;

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                if (Health.NetworkManager != null && Health.NetworkManager.IsServer && GUILayout.Button("Kill"))
                {
                    Health.Kill();
                }
            }
        }
#endif
    }
}
