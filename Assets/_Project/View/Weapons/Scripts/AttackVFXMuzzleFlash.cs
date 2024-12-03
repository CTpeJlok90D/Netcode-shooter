using Core.Weapons;
using UnityEngine;
using UnityEngine.VFX;

namespace View.Items
{
    public class AttackVFXMuzzleFlash : MonoBehaviour
    {
        [SerializeField] private VisualEffect[] _visualEffects;
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
            foreach (VisualEffect effect in _visualEffects) 
            {
                effect.Play();
            }
        }
    }
}