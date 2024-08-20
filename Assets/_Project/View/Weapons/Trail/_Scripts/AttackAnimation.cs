using Core.Weapons;
using UnityEngine;

namespace View.Weapons.Tracer
{
    public class AttackAnimation : MonoBehaviour
    {
        [field: SerializeField] public WeaponLocalReference WeaponReference { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] private string AnimationName { get; set; } = "Attack";

        public void OnEnable()
        {
            WeaponReference.Attacked += OnAttack;
        }

        public void OnDisable()
        {
            WeaponReference.Attacked -= OnAttack;
        }

        private void OnAttack()
        {
            Animator.Play(AnimationName);
        }
    }
}
