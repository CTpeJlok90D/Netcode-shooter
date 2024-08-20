using System;
using UnityEngine;

namespace Core.HealthSystem
{
    [Serializable]
    public struct DamageInfo
    {
        public GameObject Sender;
        public float Count;

        public DamageInfo(GameObject sender, float count)
        {
            Sender = sender;
            Count = count;
        }
    }
}
