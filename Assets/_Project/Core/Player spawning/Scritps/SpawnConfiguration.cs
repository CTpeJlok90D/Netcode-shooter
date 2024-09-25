using System;
using Unity.Netcode;
using UnityEngine;

namespace Core.PlayerSpawning
{
    [Serializable]
    public struct SpawnConfiguration : INetworkSerializeByMemcpy 
    {
        public NetworkObjectReference player;
        public Vector3 spawnPosition;
        public ulong playerID;
        public WeaponType mainWeapon;
        public WeaponType addictionalWeapon;
    }
}
