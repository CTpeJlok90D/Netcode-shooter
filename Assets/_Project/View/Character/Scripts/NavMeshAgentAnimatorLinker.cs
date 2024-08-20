using Core.Characters;
using UnityEngine;

namespace View.Characters
{
    public class NavMeshAgentAnimatorLinker : MonoBehaviour
    {
        [SerializeField] private TopdownCharacter _characterController;
        [SerializeField] private Animator _animator;
        [SerializeField] private string _zMovementParametrName = "Z Movement";
        [SerializeField] private string _xMovementParametrName = "X Movement";
        [SerializeField] private string _moveSpeedParametrName = "Move speed";

        private void LateUpdate()
        {
            Vector3 velocity = _characterController.Velocity;
            Vector3 localVelocity = _characterController.transform.InverseTransformDirection(velocity);
            _animator.SetFloat(_zMovementParametrName, localVelocity.z);
            _animator.SetFloat(_xMovementParametrName, localVelocity.x);
            _animator.SetFloat(_moveSpeedParametrName, localVelocity.magnitude);
        }
    }
}