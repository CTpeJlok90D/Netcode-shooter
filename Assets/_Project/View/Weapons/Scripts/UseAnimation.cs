using Core.Items;
using UnityEngine;

namespace View.Items
{
    public class UseAnimation : MonoBehaviour
    {
        [field: SerializeField] public UsebleReference UsebleReference { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public string AttackTriggerName { get; private set; } = "Attacked";
        
        private void OnEnable()
        {
            UsebleReference.Changed += OnWeaponChange;
            UsebleReference.Used += OnWeaponAttack;
        }

        private void OnDisable()
        {
            UsebleReference.Changed -= OnWeaponChange;
            UsebleReference.Used -= OnWeaponAttack;
        }

        private void OnWeaponAttack()
        {
            Animator.SetTrigger(AttackTriggerName);
        }

        private void OnWeaponChange(Useble useble)
        {
            useble.View.UseAnimation.LoadAssetAsync<AnimatorOverrideController>().Completed += (handle) => 
            {
                Animator.runtimeAnimatorController = handle.Result;
            };
        }
    }
}
