using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.ButtonPanel
{
    public class ButtonPanel : MonoBehaviour
    {
        [SerializeField] private InputActionReference _inputActionReference;
        [SerializeField] private GameObject _panel;
        [SerializeField] private bool _holdToKeepOpen;

        private void Awake()
        {
            _inputActionReference.action.Enable();
        }

        private void OnEnable()
        {
            _inputActionReference.action.started += OnButtonDown;
            _inputActionReference.action.canceled += OnButtonUp;
        }

        private void OnDisable()
        {
            _inputActionReference.action.started -= OnButtonDown;
            _inputActionReference.action.canceled -= OnButtonUp;
        }

        private void OnButtonUp(InputAction.CallbackContext context)
        {
            if (_holdToKeepOpen == false) 
            {
                return;
            }

            _panel.SetActive(false);
        }

        private void OnButtonDown(InputAction.CallbackContext context)
        {
            _panel.SetActive(_panel.activeSelf == false);
        }
    }
}
