using UnityEngine;
using UnityEngine.Playables;
using CoreHealth = Core.HealthSystem.Health;

namespace View.HealthSystem
{
    public class HitAnimationTimeline : MonoBehaviour
    {
        [SerializeField] private CoreHealth _health;
        [SerializeField] private PlayableDirector _playebleDirector;
        [SerializeField] private PlayableAsset _hitTimeLine;

        private void OnEnable()
        {
            _health.Changed += OnHealthChange;
        }

        private void OnDisable()
        {
            _health.Changed -= OnHealthChange;
        }

        private void OnHealthChange(float previousValue, float newValue)
        {
            if (newValue == 0) 
            {
                return;
            }
            if (previousValue > newValue) 
            {
                _playebleDirector.playableAsset = _hitTimeLine;
                _playebleDirector.Play();
            }
        }
    }
}
