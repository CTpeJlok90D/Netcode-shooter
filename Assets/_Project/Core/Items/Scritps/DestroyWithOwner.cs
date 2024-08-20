using Unity.Netcode;
using UnityEngine;

namespace Core.Items
{
    public class DestroyWithOwner : MonoBehaviour
    {
        [field: SerializeField] public Useble Useble { get; private set; }

        private void Update()
        {
            if (NetworkManager.Singleton.IsServer == false)
            {
                return;
            }

            if (Useble.Owner == null)
            {
                Useble.NetworkObject.Despawn(true);
            }
        }
    }
}
