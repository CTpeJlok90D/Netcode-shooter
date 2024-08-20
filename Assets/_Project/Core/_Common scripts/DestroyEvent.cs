using UnityEngine;

namespace Core
{
    public class DestroyEvent : MonoBehaviour
    {
        public delegate void DestroyEventListener(GameObject destroyedObject);
        public event DestroyEventListener Destroyed;

        public bool IsDestroyed { get; private set; } = false;

        private void OnDestroy()
        {
            IsDestroyed = true;
            Destroyed?.Invoke(gameObject);
        }
    }
}