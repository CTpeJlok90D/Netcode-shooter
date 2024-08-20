using Core.Items;
using Core.Weapons;
using UnityEngine;

namespace View.Characters
{
    public class IsAimingAnimation : MonoBehaviour
    {
        [field: SerializeField] public UsebleReference UsebleReference { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] private string IsAimingBoolParametrName { get; set; }

        public Useble Useble => UsebleReference.Value;
        private Aimable _aimeble;

        private void OnEnable()
        {
            UsebleReference.Changed += OnWeaponChange;
        }

        private void OnDisable()
        {
            UsebleReference.Changed -= OnWeaponChange;
        }

        private void OnWeaponChange(Useble usable)
        {
            if (_aimeble != null)
            {
                _aimeble.Aimed -= OnAim;
                _aimeble.AimStopped -= OnAimStop;
            }

            _aimeble = usable.GetComponent<Aimable>();
            
            if (_aimeble != null)
            {
                _aimeble.Aimed += OnAim;
                _aimeble.AimStopped += OnAimStop;
            }
        }

        private void OnAim()
        {
            Animator.SetBool(IsAimingBoolParametrName, true);
        }

        private void OnAimStop()
        {
            Animator.SetBool(IsAimingBoolParametrName, false);
        }
    }
}