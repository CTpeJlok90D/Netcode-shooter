using Core.HealthSystem;
using UnityEngine;
using UnityEngine.Playables;
using CoreHealth = Core.HealthSystem.Health;

namespace View.HealthSystem
{
    public class DeathAnimation : MonoBehaviour
    {
        [SerializeField] private CoreHealth _health;
        [SerializeField] private PlayableDirector _playebleDirector;
        [SerializeField] private PlayableAsset _dealthTimeLine;
        [SerializeField] private Animator _animator;
        [SerializeField] private string _deathTriggerName = "Death";
        [SerializeField] private string _deathDirectionXName = "Death direction X";
        [SerializeField] private string _deathDirectionZName = "Death direction Z";
        [SerializeField] private float _destroyDelay = 60f;
        [SerializeField] private Object[] _objectsToDestroyOnDeath;
        
        private void OnEnable()
        {
            _health.Death += OnDeath;
        }

        private void OnDisable()
        {
            _health.Death -= OnDeath;
        }

        private void OnDeath()
        {
            foreach (Object obj in _objectsToDestroyOnDeath) 
            {
                Destroy(obj);
            }

            DamageInfo damageInfo = _health.LastDamageInfo;
            Vector3 killerPosition;
            Vector3 killDierction;
            if (damageInfo.Sender != null)
            {
                killerPosition = damageInfo.Sender.transform.position;
                killDierction = (killerPosition - transform.position).normalized;
                killDierction.x = Mathf.RoundToInt(killDierction.x);
                killDierction.z = Mathf.RoundToInt(killDierction.z);
            }
            else 
            {
                killerPosition = Vector3.zero;
                killDierction = Vector3.zero;
            }

            _animator.applyRootMotion = true;
            _animator.SetFloat(_deathDirectionXName, killDierction.x);
            _animator.SetFloat(_deathDirectionZName, killDierction.z);
            _animator.SetTrigger(_deathTriggerName);

            _playebleDirector.transform.SetParent(null);
            _playebleDirector.playableAsset = _dealthTimeLine;
            _playebleDirector.Play();

            Destroy(_playebleDirector.gameObject, _destroyDelay);
        }
    }
}
