using System;
using Core.Characters;
using UnityEngine;
using UnityEngine.Audio;

namespace View.Characters
{
    public class FootstepSoundMaker : MonoBehaviour
    {
        [SerializeField] private AudioResource _defualtSound;
        [SerializeField] private AudioSource _source;
        [SerializeField] private TopdownCharacter _characterController;
        [SerializeField] private AnimationCurve _volumePerVelocity;
        [SerializeField] private AnimationCurve _distancePerVelocity;
        [SerializeField] private float _muteTime = 0.25f;

        private bool _mute;
        private float _defualtMaxDistance;
        private float _defualtMinDistance;
        private float _defualtVolume;

        private void Awake()
        {
            _defualtMaxDistance = _source.maxDistance;
            _defualtMinDistance = _source.minDistance;
            _defualtVolume = _source.volume;
        }

        public void OnFootstep()
        {
            if (_mute)
            {
                return;
            }

            
            float moveSpeed = _characterController.Velocity.magnitude;
            float distance = _volumePerVelocity.Evaluate(moveSpeed);
            float volume = _distancePerVelocity.Evaluate(moveSpeed);

            _source.maxDistance = distance * _defualtMaxDistance;
            _source.minDistance = distance * _defualtMinDistance;
            _source.volume = _defualtVolume * volume;

            _source.resource = _defualtSound;
            _source.Play();
            SilenceTime();
        }

        private async void SilenceTime()
        {
            try
            {
                _mute = true;
                await Awaitable.WaitForSecondsAsync(_muteTime);
                _mute = false;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }

    public class FootstepModifier : MonoBehaviour
    {
        [field: SerializeField] public AudioResource Clip { get; private set; }
    }
}
