using Core.ObjectPulling;
using UnityEngine;

namespace View.Weapons.Tracer
{
    public class Tracer : MonoBehaviour
    {
        [field: SerializeField] public TrailRenderer TrailRenderer { get; private set; }
        private void OnDeactiveFromPull()
        {
            TrailRenderer.Clear();
        }
    }
}
