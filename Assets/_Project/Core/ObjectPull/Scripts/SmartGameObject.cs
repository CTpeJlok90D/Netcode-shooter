using UnityEngine;

namespace Core.ObjectPulling
{
    public class SmartGameObject : MonoBehaviour
    {
        internal delegate void DespawnedListener(SmartGameObject gameObject);
        internal event DespawnedListener Despawned;
        public void BackToThePull()
        {
            Despawned?.Invoke(this);
            gameObject.SendMessage(nameof(OnDeactiveFromPull));
            gameObject.SetActive(false);
        }

        internal void OnActiveFromPull() {}
        internal void OnDeactiveFromPull() {}
    }
}