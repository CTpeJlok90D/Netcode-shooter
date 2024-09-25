using System.Collections.Generic;
using UnityEngine;

namespace View 
{
    [RequireComponent(typeof(Collider))]
    public class CameraObjectHide : MonoBehaviour
    {
        [SerializeField] private Material _materialToPlace;

        private Dictionary<MeshRenderer, Material> _previewMaterials = new();

        private void OnTriggerEnter(Collider other)
        {
            foreach (MeshRenderer meshRenderer in other.GetComponentsInChildren<MeshRenderer>()) 
            {
                _previewMaterials.Add(meshRenderer, meshRenderer.material);
                meshRenderer.material = _materialToPlace;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            foreach (MeshRenderer meshRenderer in other.GetComponentsInChildren<MeshRenderer>())
            {
                Material oldMaterial = _previewMaterials[meshRenderer];
                _previewMaterials.Remove(meshRenderer);
                meshRenderer.material = oldMaterial;
            }
        }
    }
}

