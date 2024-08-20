using Core.Weapons;
using UnityEngine;

namespace View.Items
{
    public class AttackMuzzleFlash : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private WeaponLocalReference _weaponReference;

        private void OnEnable()
        {
            _weaponReference.Attacked += OnAttack;
        }   

        private void OnDisable()
        {
            _weaponReference.Attacked -= OnAttack;
        }

        private void OnAttack()
        {
            _particleSystem.Play();
        }
    }
}
