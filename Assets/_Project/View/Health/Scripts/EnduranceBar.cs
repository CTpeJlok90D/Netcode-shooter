using Core.Characters;
using UnityEngine;
using UnityEngine.UI;

namespace View.HealthSystem
{
    public class EnduranceBar : MonoBehaviour
    {
        [SerializeField] private TopdownCharacter _topdownCharacter;
        [SerializeField] private Image _image;
        private void OnEnable()
        {
            _topdownCharacter.Endurance.ValueChanged += OnTopdownCharacter;
            ValidateValue();
        }

        private void OnDisable()
        {
            _topdownCharacter.Endurance.ValueChanged -= OnTopdownCharacter;
        }

        private void OnTopdownCharacter(float previousValue, float newValue)
        {
            ValidateValue();
        }

        private void ValidateValue() 
        {
            _image.fillAmount = _topdownCharacter.Endurance.Value / _topdownCharacter.MaxEndurance;
        }
    }
}
