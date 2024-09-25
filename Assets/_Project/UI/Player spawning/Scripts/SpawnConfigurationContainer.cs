using Core.PlayerSpawning;
using System;
using UnityEngine;

namespace UI.PlayerSpawning
{
    [Serializable]
    public class SpawnConfigurationContainer
    {
        public delegate void SpawnConfigurationChangedListener(SpawnConfiguration spawnConfiguration);

        [SerializeField] private SpawnConfiguration _spawnConfiguration;

        public event SpawnConfigurationChangedListener SpawnConfigurationChanged;

        public SpawnConfiguration SpawnConfiguration
        {
            get
            {
                return _spawnConfiguration;
            }
            set
            {
                _spawnConfiguration = value;
                SpawnConfigurationChanged?.Invoke(value);
            }
        }
    }
}
