using Core.Characters;
using Core.Items;
using Core.PlayerSpawning;
using Core.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace UI.Sightmark
{
    public class Sightmark : MonoBehaviour
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public InputActionReference MousePosition { get; private set; }
        [field: SerializeField] private string FloatValueName { get; set; } = "Spread";
        [Inject] public PlayerCharacterSpawner PlayerSpawner { get; private set; }

        private TopdownCharacter _character;
        private UsebleReference _usebleReference;
        private Firearm _firearm;

        private void Awake()
        {
            Animator.speed = 0;
            MousePosition.action.Enable();
        }

        private void ValidateReferences()
        {
            if (PlayerSpawner.LocalPlayerCharacter != null)
            {
                _usebleReference = PlayerSpawner.LocalPlayerCharacter.GetComponent<UsebleReference>();
                _character = PlayerSpawner.LocalPlayerCharacter.GetComponent<TopdownCharacter>();

                _firearm = null;
                if (_usebleReference.Value != null)
                {
                    _firearm = _usebleReference.Value.GetComponent<Firearm>();
                }
            }
        }

        private void LateUpdate()
        {
            if (_usebleReference == null || _character == null || _firearm == null)
            {
                ValidateReferences();
            }

            if (_usebleReference == null || _character == null || _firearm == null)
            {
                return;
            }

            float spread = _firearm.BullectConfiguration.SpreadPerSpeed.Evaluate(_character.Velocity.magnitude);
            Animator.SetFloat(FloatValueName, spread);
            transform.position = MousePosition.action.ReadValue<Vector2>();
        }
    }
}
