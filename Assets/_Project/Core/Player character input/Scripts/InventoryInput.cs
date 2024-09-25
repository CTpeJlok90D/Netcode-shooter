using Core.Character;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.PlayerCharacterInput
{
    public class InventoryInput : MonoBehaviour
    {
        [field: SerializeField] public Inventory Inventory { get; private set; }
        [field: SerializeField] public InputActionReference SelectMainWeaponActionReference { get; private set; }
        [field: SerializeField] public InputActionReference SelectAddictionalWeaponActionReference { get; private set; }

        public InputAction SelectMainWeaponAction => SelectMainWeaponActionReference.action;
        public InputAction SelectAddictionalWeaponAction => SelectAddictionalWeaponActionReference.action;

        private void Awake()
        {
            SelectMainWeaponAction.Enable();
            SelectAddictionalWeaponAction.Enable();
        }

        private void OnEnable()
        {
            SelectMainWeaponAction.canceled += SelectMainWeapon;
            SelectAddictionalWeaponAction.canceled += SelectAddictionalWeapon;
        }

        private void OnDisable()
        {
            SelectMainWeaponAction.canceled -= SelectMainWeapon;
            SelectAddictionalWeaponAction.canceled -= SelectAddictionalWeapon;
        }

        private void SelectAddictionalWeapon(InputAction.CallbackContext context)
        {
            if (Inventory.AddictionalWeapon.Item != null) 
            {
                Inventory.SelectWeapon(Inventory.AddictionalWeapon.Item);
            }
        }

        private void SelectMainWeapon(InputAction.CallbackContext context)
        {
            if (Inventory.MainWeapon.Item != null)
            {
                Inventory.SelectWeapon(Inventory.MainWeapon.Item);
            }
        }
    }
}
