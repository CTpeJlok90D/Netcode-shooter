using Core.Items;
using Core.Weapons;
using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

namespace View.Items
{
    public class CameraShootShake : MonoBehaviour
    {
        [SerializeField] private CinemachineBasicMultiChannelPerlin _camera;
        [SerializeField] private float _shakeTime = 0.13f;
        [SerializeField] private UsebleReference _usebleReference;
        [SerializeField] private Firearm _weapon;
        [SerializeField] private WeaponLocalReference _weaponReference;

        private float _shakeTimeRuntime;
        private Coroutine _coroutine;

        private void Awake()
        {
            _camera.enabled = false;
        }

        private void OnEnable()
        {
            if (_weapon != null) 
            {
                _weapon.Attacked += OnAttack;
            }
            if (_weaponReference != null) 
            {
                _weaponReference.Attacked += OnAttack;
            }
            if (_usebleReference != null) 
            {
                _usebleReference.Changed += OnItemChange;
            }
        }

        private void OnDisable()
        {
            if (_weapon != null) 
            {
                _weapon.Attacked -= OnAttack;
            }
            if (_weaponReference != null) 
            {
                _weaponReference.Attacked -= OnAttack;
            }
            if (_usebleReference != null)
            {
                _usebleReference.Changed -= OnItemChange;
            }
        }

        private void Start()
        {
            if (_usebleReference != null)
            {
                OnItemChange(_usebleReference.Value);
            }
        }

        private void OnItemChange(Useble newValue)
        {
            if (_weapon != null) 
            {
                _weapon.Attacked -= OnAttack;
            }

            if (newValue == null) 
            {
                _weapon = null;
                return;
            }
            _weapon = newValue.GetComponent<Firearm>();

            if (_weapon != null)
            {
                _weapon.Attacked += OnAttack;
            }
        }

        private void OnItemUse()
        {
            if (_weapon != null) 
            {
                OnAttack();
            }
        }

        private void OnAttack()
        {
            _shakeTimeRuntime = _shakeTime;
            if (_coroutine == null) 
            {
                _coroutine = StartCoroutine(ShakeCoroutine());
            }
        }

        private IEnumerator ShakeCoroutine() 
        {
            _camera.enabled = true;
            while (_shakeTimeRuntime > 0) 
            {
                _shakeTimeRuntime -= Time.deltaTime;
                yield return null;
            }
            _camera.enabled = false;
            _coroutine = null;
        }
    }
}
