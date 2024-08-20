using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public class DestroyObserver : MonoBehaviour
    {
        [field: SerializeField] public DestroyEvent Target { get; private set; }

        private void Awake()
        {
            Target.Destroyed += OnTargetDestroy;
        }

        private void OnDestroy()
        {
            if (Target == false)
            {
                Target.Destroyed -= OnTargetDestroy;
            }
        }

        private void OnTargetDestroy(GameObject destroyedObject)
        {
            Destroy(gameObject);
        }
    }
}