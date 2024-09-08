using Core.Weapons;
using Unity.Netcode;
using UnityEngine;

namespace View.Items
{
    public class ItemOwnerObject : MonoBehaviour
    {
        [SerializeField] private WeaponLocalReference _weaponReference;
        [SerializeField] private GameObject[] _targets;
        [SerializeField] private bool _inverse;

        private Firearm _weapon;

        private void OnEnable()
        {
            _weapon = _weaponReference.Value;
            _weaponReference.Changed += OnWeaponChange;
            if (_weaponReference.Value != null) 
            {
                _weaponReference.Value.Useble.OwnerChanged += OnOwnerChange;
                _weaponReference.Value.DestroyEvent.Destroyed += OnTargetDestroy;
            }
            if (didStart) 
            {
                ValidateActive();
            }
        }

        private void Start()
        {
            ValidateActive();
        }

        private void OnDisable()
        {
            _weaponReference.Changed -= OnWeaponChange;
            if (_weaponReference.Value != null)
            {
                _weaponReference.Value.Useble.OwnerChanged -= OnOwnerChange;
                _weaponReference.Value.DestroyEvent.Destroyed -= OnTargetDestroy;
            }
        }

        private void OnWeaponChange(Firearm weapon)
        {
            if (_weaponReference.Value != null)
            {
                _weaponReference.Value.Useble.OwnerChanged -= OnOwnerChange;
                _weaponReference.Value.DestroyEvent.Destroyed -= OnTargetDestroy;
            }
            _weapon = _weaponReference.Value;
            if (_weaponReference.Value != null)
            {
                _weaponReference.Value.Useble.OwnerChanged += OnOwnerChange;
                _weaponReference.Value.DestroyEvent.Destroyed += OnTargetDestroy;
            }
            ValidateActive();
        }

        private void OnTargetDestroy(GameObject destroyedObject)
        {
            foreach (GameObject gameObject in _targets)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnOwnerChange()
        {
            ValidateActive();
        }

        private void ValidateActive() 
        {
            if (_weaponReference.Value == null)
            {
                foreach (GameObject gameObject in _targets)
                {
                    gameObject.SetActive(false);
                }
                return;
            }

            bool result = true;
            if (_weaponReference.Value.Owner == null)
            {
                result = false;
            }
            else 
            {
                NetworkObject networkObject = _weaponReference.Value.Owner.GetComponent<NetworkObject>();
                if (networkObject.IsOwner == false) 
                {
                    result = false;
                }
            }
            if (_inverse) 
            {
                result = !result;
            }

            foreach (GameObject gameObject in _targets)
            {
                gameObject.SetActive(result);
            }
        }
    }
}
