using Core.HealthSystem;
using Core.Weapons;
using UnityEngine;

namespace View.Items
{
    public class HitSound : MonoBehaviour
    {
        [SerializeField] private AudioSource _source;
        [SerializeField] private WeaponLocalReference _weaponLocalReference;

        private void OnEnable()
        {
            _weaponLocalReference.Shot += OnShot;
        }

        private void OnDisable()
        {
            _weaponLocalReference.Shot -= OnShot;
        }

        private void OnShot(ShotInfo info)
        {
            if (info.hittenObject != null && info.hittenObject.TryGetComponent(out Health health)) 
            {
                _source.Play();
            }
        }
    }
}
