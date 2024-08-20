using Core.HealthSystem;
using UnityEngine;

namespace Core
{
    public class KillBox : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Health health))
            {
                health.Kill();
            }
        }
    }
}