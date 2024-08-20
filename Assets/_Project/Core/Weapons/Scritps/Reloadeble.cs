using UnityEngine;
using Unity.Netcode;
using System;

namespace Core.Weapons
{
    [RequireComponent(typeof(Firearm))]
    public abstract class Reloadeble : NetworkBehaviour
    {
        public abstract bool CanReload { get; }
        public abstract bool NeedReload { get; }
        public abstract void Reload();

        public delegate void ReloadStartedListener();
        public abstract event ReloadStartedListener ReloadStarted;
        public abstract event ReloadStartedListener ReloadCompleted;
    }
}