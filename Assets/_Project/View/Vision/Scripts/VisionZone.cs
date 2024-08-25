using System.Collections.Generic;
using UnityEngine;

namespace View.Vision
{
    [RequireComponent(typeof(Collider))]
    internal class VisionZone : MonoBehaviour
    {
        internal const int LayerIndex = 6;

        [SerializeField] private Collider _collider;
        [SerializeField] private Transform _rayOrigin;

        private List<VisionObject> _objectsInVision = new();

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out VisionObject visionObject) && visionObject.isActiveAndEnabled) 
            {
                _objectsInVision.Add(visionObject);
            }
        }

        private void Update()
        {
            foreach (VisionObject visionObject in _objectsInVision) 
            {
                foreach (VisionObjectRayPoint collider in visionObject.Colliders) 
                {
                    Vector3 direction = collider.transform.position - _rayOrigin.position;
                    Debug.DrawRay(_rayOrigin.position, direction, Color.yellow);
                    if (Physics.Raycast(_rayOrigin.position, direction, out RaycastHit hit, Mathf.Infinity))
                    {
                        if (hit.collider.gameObject == visionObject.gameObject)
                        {
                            visionObject.InVisionZone = true;
                            break;
                        }
                        visionObject.InVisionZone = false;
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out VisionObject visionObject) && visionObject.isActiveAndEnabled)
            {
                _objectsInVision.Remove(visionObject);
                visionObject.InVisionZone = false;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;

            if (gameObject.layer != LayerIndex)
            {
                gameObject.layer = LayerIndex;
            }
        }
#endif
    }
}