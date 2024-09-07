using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Core.Players
{
    public class Player : NetworkBehaviour
    {
        private static List<Player> _list = new();
        private static Player _local;
        public static Player Local => _local;
        public static IReadOnlyList<Player> List => _list;

        public static event Action<Player> LocalPlayerSpawned;
        public static event Action<Player> LocalPlayerDespawned;
        public static event Action<Player> Join;
        public static event Action<Player> Left;

        public bool IsLocal => this == Local;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            _list.Add(this);
            if (IsLocalPlayer)
            {
                _local = this;
                LocalPlayerSpawned?.Invoke(this);
            }

            Join?.Invoke(this);
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            _list.Remove(this);
            if (this == Local) 
            {
                LocalPlayerDespawned?.Invoke(this);
            }
            Left?.Invoke(this);
        }
    }
}
