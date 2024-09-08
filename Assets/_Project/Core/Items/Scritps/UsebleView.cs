using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.Items
{
    [Serializable]
    public class UsebleView
    {
        [field: SerializeField] public AssetReference ViewReference { get; private set; }
        [field: SerializeField] public AnimatorOverrideController UseAnimation { get; private set; }
    }
}
