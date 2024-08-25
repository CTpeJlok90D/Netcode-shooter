using UnityEngine;
using UnityEngine.UI;

namespace View.Health
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;
        [SerializeField] private Core.HealthSystem.Health _health;

        private void OnEnable()
        {
            _health.Changed += OnHealthValueChange;
            if (didStart)
            {
                ValidateFill();
            }
        }

        private void Start()
        {
            ValidateFill();
        }

        private void OnDisable()
        {
            _health.Changed -= OnHealthValueChange;
        }

        private void OnHealthValueChange(float previousValue, float newValue) => ValidateFill();
        private void ValidateFill()
        {
            _fillImage.fillAmount = _health.Value / _health.Max;
        }
    }
}
