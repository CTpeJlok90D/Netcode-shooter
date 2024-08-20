using Core.Items;
using Core.Weapons;
using UnityEngine;

namespace View.Characters
{
    public class ReloadingAnimation : MonoBehaviour
    {
        [field: SerializeField] public UsebleReference UsebleReference { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] private string ReloadStartedTriggerName { get; set; } = "Reloading";
        [field: SerializeField] private string ReloadCompletedTriggerName { get; set; } = "Reload completed";

        public Useble Useble => UsebleReference.Value;
        private Reloadeble _aimeble;

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
                _aimeble.ReloadStarted -= OnReloadStart;
                _aimeble.ReloadCompleted -= OnReloadCompleted;
            }

            _aimeble = usable.GetComponent<Reloadeble>();
            
            if (_aimeble != null)
            {
                _aimeble.ReloadStarted += OnReloadStart;
                _aimeble.ReloadCompleted += OnReloadCompleted;
            }
        }

        private void OnReloadStart()
        {
            Animator.SetTrigger(ReloadStartedTriggerName);
        }

        private void OnReloadCompleted()
        {
            Animator?.SetTrigger(ReloadCompletedTriggerName);
        }
    }
}