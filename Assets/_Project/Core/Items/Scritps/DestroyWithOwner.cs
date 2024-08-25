using System;
using Unity.Netcode;
using UnityEngine;

namespace Core.Items
{
    public class DestroyWithOwner : MonoBehaviour
    {
        [field: SerializeField] public Useble Useble { get; private set; }

        private void OnEnable()
        {
            Useble.OwnerChanged += OnOwnerChange;
        }

        private void OnDisable()
        {
            Useble.OwnerChanged -= OnOwnerChange;
        }

        private void OnOwnerChange()
        {
            if (Useble.Owner == null) 
            {
                Useble.NetworkObject.Despawn(true);
            }
        }
    }
}