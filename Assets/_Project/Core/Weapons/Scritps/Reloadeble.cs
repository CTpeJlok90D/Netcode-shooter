using UnityEngine;
using Unity.Netcode;

namespace Core.Weapons
{
    [RequireComponent(typeof(Firearm))]
    [Icon("Assets/_Project/Core/Weapons/Editor/Icons/icons8-rifle-magazine-96.png")]
    public abstract class Reloadeble : NetworkBehaviour
    {
        public abstract bool CanReload { get; }
        public abstract bool NeedReload { get; }
        public abstract void Reload();
        public abstract void BrokeReload();

        public delegate void ReloadStartedListener();
        public abstract event ReloadStartedListener ReloadStarted;
        public abstract event ReloadStartedListener ReloadCompleted;
    }
}