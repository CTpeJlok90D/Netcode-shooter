using System.Collections.Generic;
using UnityEngine;

namespace Core.ObjectPulling
{
    public class SmartGameObjectPull
    {
        private SmartGameObject _gameObject_PREFAB;
        private List<SmartGameObject> _activeInstances = new();
        private List<SmartGameObject> _inactiveInstances= new();

        public SmartGameObjectPull(SmartGameObject gameObject_PREFAB)
        {
            _gameObject_PREFAB = gameObject_PREFAB;
        }

        public void Clear()
        {
            foreach (SmartGameObject gameObject in _activeInstances)
            {
                Object.Destroy(gameObject.gameObject);
            }
            foreach (SmartGameObject gameObject in _inactiveInstances)
            {
                Object.Destroy(gameObject.gameObject);
            }
        }

        public SmartGameObject Instantiate()
        {
            if (_inactiveInstances.Count == 0)
            {
                SmartGameObject instance = Object.Instantiate(_gameObject_PREFAB);
                instance.Despawned += OnDespawn;
                instance.gameObject.SendMessage(nameof(SmartGameObject.OnActiveFromPull));

                _activeInstances.Add(instance);

                return instance;
            }
            SmartGameObject result = _inactiveInstances[0];
            _inactiveInstances.Remove(result);
            _activeInstances.Add(result);

            result.gameObject.SetActive(true);
            result.gameObject.SendMessage(nameof(SmartGameObject.OnActiveFromPull));

            return result;
        }

        private void OnDespawn(SmartGameObject gameObject)
        {
            _inactiveInstances.Add(gameObject);
            _activeInstances.Remove(gameObject);
        }
    }
}