using System.Linq;
using UnityEngine;

namespace View.Vision
{
    internal class VisionObject : MonoBehaviour 
    {
        internal const int LayerIndex = 7;

        [SerializeField] private Renderer[] _meshRenderers;
        [SerializeField] private MonoBehaviour[] _components;
        [SerializeField] private GameObject[] _gameObjects;
        [SerializeField] private bool _autoFillRenderers;
        [field: SerializeField] internal VisionObjectRayPoint[] Colliders { get; private set; }

        private bool _inVisionZone;

        public bool InVisionZone 
        {
            get
            {
                return _inVisionZone;
            }
            set 
            {
                if (enabled) 
                {
                    if (value) 
                    {
                        Show();
                    }
                    else 
                    {
                        Hide();
                    }
                }
                _inVisionZone = value;
            }
        }

        private void OnEnable()
        {
            InVisionZone = _inVisionZone;
        }

        private void OnDisable()
        {
            Show();
        }

        public void Show() 
        {
            foreach (Renderer mesh in _meshRenderers) 
            {
                mesh.enabled = true;
            }
            foreach (MonoBehaviour component in _components) 
            {
                component.enabled = true;
            }
            foreach (GameObject gameObject in _gameObjects)
            {
                gameObject.SetActive(true);
            }
        }

        public void Hide() 
        {
            foreach (Renderer mesh in _meshRenderers)
            {
                mesh.enabled = false;
            }
            foreach (MonoBehaviour component in _components)
            {
                component.enabled = false;
            }
            foreach (GameObject gameObject in _gameObjects)
            {
                gameObject.SetActive(false);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if ((_meshRenderers == null || _meshRenderers.Count() == 0) && _autoFillRenderers) 
            {
                _meshRenderers = GetComponentsInChildren<Renderer>();
            }
            if (gameObject.layer != LayerIndex) 
            {
                gameObject.layer = LayerIndex;
            }
        }
#endif
    }
}