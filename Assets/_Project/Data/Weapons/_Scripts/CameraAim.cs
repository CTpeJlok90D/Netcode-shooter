using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Data.Weapons
{
    public class CameraAim : SimpleAim
    {
        [SerializeField] private CinemachineCamera _camera;
        [SerializeField] private Camera _localCamera;
        [SerializeField] private InputActionReference _cursorPosition;
        [SerializeField] private Transform _lookTransform;
        [SerializeField] private int _noAimPriority = -1;
        [SerializeField] private int _aimPriority = 100;

        private CinemachineBrain _brain;

        protected override void Awake()
        {
            base.Awake();
            _cursorPosition.action.Enable();
            _brain = Camera.main.GetComponent<CinemachineBrain>();
        }

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
            Vector2 cursorPosition = _cursorPosition.action.ReadValue<Vector2>();
            Ray ray = _localCamera.ScreenPointToRay(cursorPosition);

            Vector3 endPosition = Vector3.zero;
            if (Physics.Raycast(ray, out RaycastHit hitInfo)) 
            {
                endPosition = hitInfo.point;
            }

            if (Vector3.Distance(Weapon.transform.position, endPosition) < Weapon.BullectConfiguration.MaxDistance)
            {
                _lookTransform.position = endPosition;
            }
            else
            {
                Vector3 direction = (endPosition - Weapon.transform.position).normalized;
                Vector3 offcet = direction * Weapon.BullectConfiguration.MaxDistance;
                _lookTransform.position = Weapon.transform.position + offcet;
            }
        }
    }
}
