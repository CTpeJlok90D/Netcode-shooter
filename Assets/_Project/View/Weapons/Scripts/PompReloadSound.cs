using Core.Weapons;
using UnityEngine;

namespace View.Items
{
    public class PompReloadSound : ReloadSound
    {
        private PompReload _pompReload;
        [field: SerializeField] public AudioSource BulletInjectedSource { get; private set; }

        protected override void OnEnable()
        {
            base.OnEnable();

            _pompReload = Reloadeble as PompReload;
            if (Reloadeble != null && Reloadeble is PompReload)
            {
                _pompReload.BulletInjected += OnBulletInject;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (Reloadeble != null && Reloadeble is PompReload)
            {
                _pompReload.BulletInjected -= OnBulletInject;
            }
        }

        protected override void OnWeaponChange(Firearm weapon)
        {
            base.OnWeaponChange(weapon);

            if (_pompReload != null) 
            {
                _pompReload.BulletInjected -= OnBulletInject;
            }

            _pompReload = Reloadeble as PompReload;

            if (_pompReload != null)
            {
                _pompReload.BulletInjected += OnBulletInject;
            }
        }

        private void OnBulletInject()
        {
            BulletInjectedSource?.Play();
        }
    }
}
