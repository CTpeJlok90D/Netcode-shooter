using UnityEngine;

namespace Core.Items
{
    public class UsebleReference : ComponentNetworkReference<Useble>
    {
        [field: SerializeField] public bool IsItemOwer { get; private set; }

        protected override void OnValueChange()
        {
            base.OnValueChange();
            if (IsItemOwer)
            {
                if (Value == null)
                {
                    return;
                }
                if (NetworkManager.IsServer)
                {
                    Value.SetOwner(NetworkObject);
                }
                Value.transform.position = transform.position;
                Value.transform.rotation = transform.rotation;
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (NetworkManager.IsServer)
            {
                Value.SetOwner(null);
            }
        }
    }
}