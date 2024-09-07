using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Weapons
{
    public class WeaponLocalReference : MonoBehaviour
    {
        [SerializeField] private Firearm _weapon;
        
        private List<Action> _attackedListeners = new();
        private List<Bullet.ShotListener> _shotListeners = new();
        private List<Action> _dryFireListeners = new();

        public delegate void WeaponChangedListener(Firearm weapon);
        public event WeaponChangedListener Changed;

        public Firearm Value 
        {
            get
            {
                return _weapon;
            } 
            set
            {
                GameObject oldOwner = null;
                if (_weapon != null)
                {
                    oldOwner = _weapon.Owner;
                    _weapon.Attacked -= OnAttack;
                    _weapon.AttackPattern.Bullet.Shot -= OnShot;
                    _weapon.AttackPattern.DryFire -= OnDryFire;
                }

                _weapon = value;

                if (_weapon != null)
                {
                    _weapon.Attacked += OnAttack;
                    _weapon.AttackPattern.Bullet.Shot += OnShot;
                    _weapon.AttackPattern.DryFire += OnDryFire;
                }

                Changed?.Invoke(_weapon);
            }
        }

        public event Action Attacked
        {
            add => _attackedListeners.Add(value);
            remove => _attackedListeners.Remove(value);
        }

        public event Action DryFire 
        {
            add => _dryFireListeners.Add(value);
            remove => _dryFireListeners.Remove(value);
        }

        public event Bullet.ShotListener Shot
        {
            add => _shotListeners.Add(value);
            remove => _shotListeners.Remove(value);
        }

        private void OnEnable()
        {
            if (_weapon != null)
            {
                _weapon.Attacked += OnAttack;
                _weapon.AttackPattern.Bullet.Shot += OnShot;
                _weapon.AttackPattern.DryFire += OnDryFire;
            }
        }

        private void OnDisable()
        {
            if (_weapon != null)
            {
                _weapon.Attacked -= OnAttack;
                _weapon.AttackPattern.Bullet.Shot -= OnShot;
                _weapon.AttackPattern.DryFire -= OnDryFire;
            }
        }

        private void OnDryFire()
        {
            foreach (Action action in _dryFireListeners)
            {
                action.Invoke();
            }
        }

        private void OnShot(ShotInfo info)
        {
            foreach (Bullet.ShotListener action in _shotListeners)
            {
                action.Invoke(info);
            }
        }

        private void OnAttack()
        {
            foreach (Action action in _attackedListeners)
            {
                action.Invoke();
            }
        }
    }
}