using Core.HealthSystem;
using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public class KillBox : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (NetworkManager.Singleton.IsServer == false) { return; }
            if (other.gameObject.TryGetComponent(out Health health))
            {
                health.Kill();
            }
        }
    }
}