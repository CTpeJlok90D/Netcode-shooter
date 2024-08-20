using UnityEngine;

namespace Core
{
    public class NoParentAwake : MonoBehaviour
    {
        private void Awake()
        {
            transform.SetParent(null);
        }
    }
}