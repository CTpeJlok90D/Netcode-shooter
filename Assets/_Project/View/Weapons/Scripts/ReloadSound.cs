using Core.Weapons;
using UnityEngine;

namespace View.Items
{
    public class ReloadSound : MonoBehaviour
    {
        [field: SerializeField] private WeaponLocalReference WeaponLocalReference { get; set; }
        [field: SerializeField] public AudioSource ReloadAudioSource { get; private set; }
        [field: SerializeField] public AudioSource ReloadCompletedSource { get; private set; }
        
        public Reloadeble Reloadeble { get; private set; }

        protected virtual void OnEnable()
        {
            if (WeaponLocalReference.Value != null)
            {
                Reloadeble = WeaponLocalReference.Value.GetComponent<Reloadeble>();
            }

            WeaponLocalReference.Changed += OnWeaponChange;
            if (Reloadeble != null)
            {
                Reloadeble.ReloadStarted += OnReload;
                Reloadeble.ReloadCompleted += OnReloadComplete;
            }
        }

        protected virtual void OnDisable()
        {
            WeaponLocalReference.Changed -= OnWeaponChange;   
            if (Reloadeble != null)
            {
                Reloadeble.ReloadStarted -= OnReload;
                Reloadeble.ReloadCompleted -= OnReloadComplete;
            }
        }

        protected virtual void OnWeaponChange(Firearm weapon)
        {
            if (Reloadeble != null)
            {
                Reloadeble.ReloadStarted -= OnReload;
                Reloadeble.ReloadCompleted -= OnReloadComplete;
            }

            Reloadeble = weapon.GetComponent<Reloadeble>();

            if (Reloadeble != null)
            {
                Reloadeble.ReloadStarted += OnReload;
                Reloadeble.ReloadCompleted += OnReloadComplete;
            }
        }

        private void OnReloadComplete()
        {
            if (ReloadCompletedSource != null) 
            {
                ReloadCompletedSource.Play();
            }
        }

        private void OnReload()
        {
            if (ReloadAudioSource != null)
            {
                ReloadAudioSource?.Play();
            }
        }
    }
}
