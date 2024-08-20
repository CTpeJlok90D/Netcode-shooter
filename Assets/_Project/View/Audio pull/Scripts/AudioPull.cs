using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace View
{    
    public class AudioPull : MonoBehaviour
    {
        [SerializeField] private AudioSource _source_PREFAB;
        
        private List<AudioSource> _availableAudioSources = new();

        public async Task Play(AudioResource resource, float clipLenght)
        {
            try
            {
                if (_availableAudioSources.Count == 0)
                {
                    AudioSource instance = Instantiate(_source_PREFAB, transform);
                    _availableAudioSources.Add(instance);
                }

                AudioSource audioSource = _availableAudioSources[0];
                audioSource.resource = resource;
                _availableAudioSources.Remove(audioSource);
                audioSource.Play();
                await Awaitable.WaitForSecondsAsync(clipLenght);
                _availableAudioSources.Add(audioSource);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}