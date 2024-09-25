using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEditor;
using UnityEngine;

namespace View.Items
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(NetworkTransform))]
    [RequireComponent(typeof(NetworkObject))]
    public class BulletHitRigidbody : NetworkBehaviour
    {
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            Rigidbody.isKinematic = true;
        }

        public void Hitten() 
        {
            if (NetworkManager.Singleton.IsServer) 
            {
                Rigidbody.isKinematic = false;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.IsPlaying(this)) 
            {
                return;
            }

            Rigidbody = GetComponent<Rigidbody>();
            Rigidbody.isKinematic = true;

            MeshCollider meshCollider;
            if (TryGetComponent(out meshCollider)) 
            {
                meshCollider.convex = true;
            }

            if (TryGetComponent(out Collider collider) == false) 
            {
                meshCollider = gameObject.AddComponent<MeshCollider>();
                meshCollider.convex = true;
            }

            if (TryGetComponent(out NetworkTransform netTransform)) 
            {
                netTransform.SyncScaleX = false;
                netTransform.SyncScaleY = false;
                netTransform.SyncScaleZ = false;
            }
        }
#endif
    }
}
