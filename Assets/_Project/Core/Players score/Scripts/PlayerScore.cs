using System;
using Unity.Netcode;

namespace Core.PlayersScore
{
    public struct PlayerScore : INetworkSerializeByMemcpy, IEquatable<PlayerScore>
    {
        public ulong PlayerID;
        public NetworkObjectReference PlayerObject;
        public int Kills;
        public int Deaths;
        public int Damage;

        public bool Equals(PlayerScore other)
        {
            return PlayerID == other.PlayerID && Kills == other.Kills && Deaths == other.Deaths && Damage == other.Damage;
        }
    }
}