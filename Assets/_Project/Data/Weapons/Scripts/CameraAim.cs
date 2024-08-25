using Core.Characters;
using Unity.Cinemachine;
using UnityEngine;

namespace Data.Weapons
{
    public class CameraAim : SimpleAim
    {
        [SerializeField] private CinemachineCamera _camera;
        [SerializeField] private Transform _lookTransform;
        [SerializeField] private int _noAimPriority = -1;
        [SerializeField] private int _aimPriority = 100;

        protected override void OnAim()
        {
            base.OnAim();
            if (_camera.isActiveAndEnabled)
            {
                _camera.Priority = _aimPriority;
            }
        }

        protected override void OnAimStop()
        {
            base.OnAimStop();
            if (_camera.isActiveAndEnabled)
            {
                _camera.Priority = _noAimPriority;
            }
        }

        private void Update()
        {
            ValidatePosition();
        }

        private void ValidatePosition() 
        {
            if (Weapon == null || Weapon.TopdownCharacter == null) return;

            if (Vector3.Distance(Weapon.transform.position, Weapon.TopdownCharacter.LookPoint) < Weapon.BullectConfiguration.MaxDistance)
            {
                _lookTransform.position = Weapon.TopdownCharacter.LookPoint;
            }
            else
            {
                Vector3 direction = (Weapon.TopdownCharacter.LookPoint - Weapon.transform.position).normalized;
                Vector3 offcet = direction * Weapon.BullectConfiguration.MaxDistance;
                _lookTransform.position = Weapon.transform.position + offcet;
            }
        }
    }
}
