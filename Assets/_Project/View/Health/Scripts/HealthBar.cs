using UnityEngine;
using UnityEngine.UI;

namespace View.Health
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;
        [SerializeField] private Core.HealthSystem.Health _health;
        [SerializeField] private float _oneHealthPointSize;

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
            Vector3 newScale = new()
            {
                x = _fillImage.transform.localScale.x * _health.Value * _oneHealthPointSize,
                y = _fillImage.transform.localScale.y,
                z = _fillImage.transform.localScale.z,
            };
            _fillImage.transform.localScale = newScale;
        }
    }
}
