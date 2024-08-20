using System;
using Unity.Netcode;

namespace Core.Characters
{
    [Serializable]
    public struct SpeedModificator : INetworkSerializeByMemcpy, IEquatable<SpeedModificator>
    {
        public float AccelerationClamp;
        public float MaxSpeedClamp;

        public bool Equals(SpeedModificator other)
        {
            return 
                AccelerationClamp == other.AccelerationClamp &&
                MaxSpeedClamp == other.MaxSpeedClamp;
        }
    }
}
