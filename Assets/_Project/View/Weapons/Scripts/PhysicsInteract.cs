using Core.Weapons;
using UnityEngine;

namespace View.Items
{
    public class PhysicsInteract : MonoBehaviour
    {
        [SerializeField] private WeaponLocalReference _weaponReference;
        [SerializeField] private float _force = 500f;

        private void OnEnable()
        {
            _weaponReference.Shot += OnShot;
        }

        private void OnDisable()
        {
            _weaponReference.Shot -= OnShot;
        }

        private void OnShot(ShotInfo info)
        {
            if (info.hittenObject != null && info.hittenObject.TryGetComponent(out BulletHitRigidbody bulletHitRB))
            {
                Vector3 direction = (info.endPosition - info.startPosition).normalized;
                Vector3 force = direction * _force;
                bulletHitRB.Hitten();
                bulletHitRB.Rigidbody.AddForce(force);
            }
        }
    }
}
