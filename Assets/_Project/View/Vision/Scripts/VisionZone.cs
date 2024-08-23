using UnityEngine;

namespace View.Vision
{
    [RequireComponent(typeof(Collider))]
    internal class VisionZone : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private int _rayLayer;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out VisionObject visionObject)) 
            {
                visionObject.InVisionZone = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out VisionObject visionObject))
            {
                visionObject.InVisionZone = false;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;
        }
#endif
    }
}