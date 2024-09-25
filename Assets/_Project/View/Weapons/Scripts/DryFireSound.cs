using Core.Weapons;
using UnityEngine;

namespace View.Items
{
    public class DryFireSound : MonoBehaviour
    {
        [SerializeField] private WeaponLocalReference _weapon;
        [SerializeField] private AudioSource _audioSource;

        private void OnEnable()
        {
            _weapon.DryFire += OnDryFire;
        }

        private void OnDisable()
        {
            _weapon.DryFire -= OnDryFire;
        }

        private void OnDryFire()
        {
            _audioSource.Play();
        }
    }
}
