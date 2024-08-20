using System;
using Core.Weapons;
using UnityEngine;

namespace View.Items
{
    public class ReloadSound : MonoBehaviour
    {
        [field: SerializeField] private WeaponLocalReference WeaponLocalReference { get; set; }
        [field: SerializeField] public AudioSource ReloadAudioSource { get; private set; }
        
        public Reloadeble Reloadeble { get; private set; }

        private void OnEnable()
        {
            if (WeaponLocalReference.Value != null)
            {
                Reloadeble = WeaponLocalReference.Value.GetComponent<Reloadeble>();
            }

            WeaponLocalReference.Changed += OnWeaponChange;
            if (Reloadeble != null)
            {
                Reloadeble.ReloadStarted += OnReload;
            }
        }

        private void OnDisable()
        {
            WeaponLocalReference.Changed -= OnWeaponChange;   
            if (Reloadeble != null)
            {
                Reloadeble.ReloadStarted -= OnReload;
            }
        }

        private void OnWeaponChange(Firearm weapon)
        {
            if (Reloadeble != null)
            {
                Reloadeble.ReloadStarted -= OnReload;
            }

            Reloadeble = weapon.GetComponent<Reloadeble>();

            if (Reloadeble != null)
            {
                Reloadeble.ReloadStarted += OnReload;
            }
        }

        private void OnReload()
        {
            ReloadAudioSource.Play();
        }
    }
}
