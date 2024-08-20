using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using TopdownCharacter = Core.Characters.TopdownCharacter;

namespace Core.PlayerCharacterInput
{
    public class PlayerCharacterInput : MonoBehaviour
    {
        [field: SerializeField] public CinemachineCamera CinemachineCamera { get; private set; }
        [field: SerializeField] public TopdownCharacter NavMeshAgent { get; private set; }
        [field: SerializeField] public InputActionReference MovementInputActionReference { get; private set; }
        [field: SerializeField] public InputActionReference LookInputActionReference { get; private set; }
        [field: SerializeField] public InputActionReference MousePositionActionReference { get; private set; }
        [field: SerializeField] public InputActionReference SitActionReference { get; private set; }
        [field: SerializeField] public InputActionReference SprintActionReference { get; private set; }

        public InputAction MovementInputAction => MovementInputActionReference.action;
        public InputAction LookInputAction => LookInputActionReference.action;
        public InputAction MousePositionAction => MousePositionActionReference.action;
        public InputAction SitAction => SitActionReference.action;
        public InputAction SprintAction => SprintActionReference.action;
        private Vector2 _input;

        private void Awake()
        {
            MovementInputAction.Enable();
            LookInputAction.Enable();
            MousePositionAction.Enable();
            SitAction.Enable();
            SprintAction.Enable();
        }

        private void OnApplicationPause(bool pause)
        {
            if (!pause)
            {
                MovementInputAction.Enable();
                LookInputAction.Enable();
                MousePositionAction.Enable();
                SitAction.Enable();
                SprintAction.Enable();
            }
            else
            {
                MovementInputAction.Disable();
                LookInputAction.Disable();
                MousePositionAction.Disable();
                SitAction.Disable();
                SprintAction.Disable();
            }
        }
        private void OnEnable()
        {
            MovementInputAction.started += OnInpuActionStart;
            MovementInputAction.performed += OnInpuActionStart;
            MovementInputAction.canceled += OnInputActionCanceled;


            SitAction.started += OnSitStart;
            SitAction.canceled += OnSitCancel;

            SprintAction.started += OnSprintStart;
            SprintAction.canceled += OnSprintStop;
        }

        private void OnDisable()
        {
            MovementInputAction.started -= OnInpuActionStart;
            MovementInputAction.performed -= OnInpuActionStart;
            MovementInputAction.canceled -= OnInputActionCanceled;

            SitAction.started -= OnSitStart;
            SitAction.canceled -= OnSitCancel;

            SprintAction.started -= OnSprintStart;
            SprintAction.canceled -= OnSprintStop;
        }

        private void Update()
        {
            Vector3 moveDirection = new Vector3(_input.x, 0, _input.y);
            NavMeshAgent.Move(moveDirection);
            ValidateLookInput();
        }

        private void OnSprintStart(InputAction.CallbackContext context)
        {
            NavMeshAgent.IsSprinting = true;
        }

        private void OnSprintStop(InputAction.CallbackContext context)
        {
            NavMeshAgent.IsSprinting = false;
        }

        private void OnSitStart(InputAction.CallbackContext context)
        {
            NavMeshAgent.IsCrouching = true;
        }

        private void OnSitCancel(InputAction.CallbackContext context)
        {
            NavMeshAgent.IsCrouching = false;
        }

        void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus == false)
            {
                NavMeshAgent.IsCrouching = false;
                NavMeshAgent.IsSprinting = false;
            }
        }

        private void ValidateLookInput()
        {
            Vector2 mousePosition = MousePositionAction.ReadValue<Vector2>();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                Vector3 hitPoint = hitInfo.point;
                NavMeshAgent.SetLookPosition(hitPoint);
            }
        }

        private void OnInpuActionStart(InputAction.CallbackContext context)
        {
            _input = context.ReadValue<Vector2>();
        }

        private void OnInputActionCanceled(InputAction.CallbackContext context)
        {
            _input = new();
        }
    }
}
