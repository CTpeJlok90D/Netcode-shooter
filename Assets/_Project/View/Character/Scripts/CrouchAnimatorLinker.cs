using Core.Characters;
using UnityEngine;

public class CrouchAnimatorLinker : MonoBehaviour
{
    [field: SerializeField] public TopdownCharacter Character { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] private string IsCrouchingBoolName { get; set; } = "Is crouching";

    private void LateUpdate()
    {
        Animator.SetBool(IsCrouchingBoolName, Character.IsCrouching);
    }
}
