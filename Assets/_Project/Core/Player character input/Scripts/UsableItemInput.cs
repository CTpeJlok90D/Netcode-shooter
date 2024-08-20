using Core.Items;
using Core.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.PlayerCharacterInput
{
    public class UsableItemInput : MonoBehaviour
    {
        [field: SerializeField] public InputActionReference AttackActionReference { get; private set; }
        [field: SerializeField] public InputActionReference AimActionReference { get; private set; }
        [field: SerializeField] public InputActionReference ReloadActionReference { get; private set; }
        [field: SerializeField] public UsebleReference UsableReference { get; private set; }

        private Aimable _aimable;
        private Reloadeble _reloadeble;

        public InputAction AttackAction => AttackActionReference;
        public InputAction AimAction => AimActionReference.action;
        public InputAction ReloadAction => ReloadActionReference.action;

        public Useble Weapon => UsableReference.Value;

        private void Awake()
        {
            AttackAction.Enable();
            AimAction.Enable();
            ReloadAction.Enable();
        }

        private void OnApplicationPause(bool pause)
        {
            if (!pause) 
            {
                AttackAction.Enable();
                AimAction.Enable();
                ReloadAction.Enable();
            }
            else
            {
                AttackAction.Disable();
                AimAction.Disable();
                ReloadAction.Disable();
            }
        }

        private void OnEnable()
        {
            UsableReference.Changed += OnUsebleChange;

            AttackAction.started += OnAttackStart;
            AttackAction.canceled += OnAttackStop;

            AimAction.started += OnAimStart;
            AimAction.canceled += OnAimACancel;

            ReloadAction.started += OnReload;
        }

        private void OnDisable()
        {
            UsableReference.Changed -= OnUsebleChange;

            AttackAction.started -= OnAttackStart;
            AttackAction.canceled -= OnAttackStop;

            AimAction.started -= OnAimStart;
            AimAction.canceled -= OnAimACancel;

            ReloadAction.started -= OnReload;
        }

        private void OnUsebleChange(Useble weapon)
        {
            if (weapon == null)
            {
                _aimable = null;
                _reloadeble = null;
                return;
            }

            _aimable = weapon.GetComponent<Aimable>();
            _reloadeble = weapon.GetComponent<Reloadeble>();
        }

        private void OnAttackStart(InputAction.CallbackContext context)
        {
            Weapon.StartAttack();
        }

        private void OnAttackStop(InputAction.CallbackContext context)
        {
            Weapon.StopAttack();
        }

        private void OnAimStart(InputAction.CallbackContext context)
        {
            if (_aimable != null)
            {
                _aimable.IsAiming = true;
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus == false)
            {
                _aimable.IsAiming = false;
                if (Weapon.IsUsing)
                {
                    Weapon.StopAttack();
                }
            }
        }

        private void OnAimACancel(InputAction.CallbackContext context)
        {
            if (_aimable != null)
            {
                _aimable.IsAiming = false;
            }
        }

        private void OnReload(InputAction.CallbackContext context)
        {
            _reloadeble?.Reload();
        }
    }
}
