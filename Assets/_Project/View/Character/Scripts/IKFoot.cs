using UnityEngine;

namespace View.Characters
{
    [RequireComponent(typeof(Animator))]
    public class IKFoot : MonoBehaviour
    {
        [SerializeField] private AvatarIKGoal _ikBone;
        [SerializeField] private Animator _animator;
        [SerializeField] private LayerMask _rayCastMask;
        [SerializeField] private float _maxDistanceToGround = 0.1f;
        [SerializeField] private Vector3 _footOffcet;

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        private void OnAnimatorIK(int layerIndex)
        {
            Vector3 ikBonePosition = _animator.GetIKPosition(_ikBone);
            Ray ray = new Ray(ikBonePosition + Vector3.up, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _rayCastMask))
            {
                _animator.SetIKPositionWeight(_ikBone, 1);
                Vector3 hitPosition = hit.point;
                if (Vector3.Distance(hitPosition, ikBonePosition) > _maxDistanceToGround)
                {
                    return;
                }

                Quaternion footRotation = Quaternion.LookRotation(transform.forward, hit.normal);
                _animator.SetIKPosition(_ikBone, hitPosition + _footOffcet);
                _animator.SetIKRotation(_ikBone, footRotation);
            }
            else
            {
                _animator.SetIKPositionWeight(_ikBone, 0);
            }
        }
    }
}