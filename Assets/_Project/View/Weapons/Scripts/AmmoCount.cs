using Core.Weapons;
using System;
using TMPro;
using UnityEngine;

namespace View.Items
{
    public class AmmoCount : MonoBehaviour
    {
        [SerializeField] private WeaponLocalReference _weapon;
        [SerializeField] private TMP_Text _tmpLabel;
        [SerializeField] private string _format = "{0}/{1}";

        private Clip _clip;

        private void OnEnable()
        {
            if (didStart) 
            {
                _clip.Ammo.ValueChanged += OnAmmoChange;
            }
            _weapon.Changed += OnWeaponChange;
        }

        private void OnDisable()
        {
            if (_clip != null)
            {
                _clip.Ammo.ValueChanged -= OnAmmoChange;
            }
            _weapon.Changed -= OnWeaponChange;
        }


        private void Start()
        {
            ValidateWeapon();
        }
        private void OnWeaponChange(Firearm weapon) => ValidateWeapon();
        private void ValidateWeapon() 
        {
            if (_clip != null) 
            {
                _clip.Ammo.ValueChanged -= OnAmmoChange;
            }

            if (_weapon.Value != null)
            {
                _clip = _weapon.Value.GetComponent<Clip>();
                _clip.Ammo.ValueChanged += OnAmmoChange;
                UpdateAmmo();
            }
        }

        private void OnAmmoChange(int previousValue, int newValue) => UpdateAmmo();
        private void UpdateAmmo() 
        {
            _tmpLabel.text = string.Format(_format, _clip.Ammo.Value, _clip.MaxAmmo);
        }
    }
}
