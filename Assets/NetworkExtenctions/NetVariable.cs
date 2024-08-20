using System;

namespace Unity.Netcode.Custom
{
    [Serializable]
    public class NetVariable<T> : NetworkVariable<T>
    {
        public event OnValueChangedDelegate ValueChanged;

        public NetVariable() : base()
        {
            OnValueChanged = OnValueChange;
        }

        public NetVariable(
            T value = default,
            NetworkVariableReadPermission readPerm = NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission writePerm = NetworkVariableWritePermission.Server) : base(value, readPerm, writePerm)
        {
            OnValueChanged = OnValueChange;
        }

        private void OnValueChange(T previousValue, T newValue)
        {
            ValueChanged?.Invoke(previousValue, newValue);
        }
    }
}
