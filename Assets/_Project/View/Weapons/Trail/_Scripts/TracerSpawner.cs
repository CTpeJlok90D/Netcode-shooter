using System;
using System.Threading.Tasks;
using Core.ObjectPulling;
using Core.Weapons;
using UnityEngine;

namespace View.Weapons.Tracer
{
    public class TracerSpawner : MonoBehaviour
    {
        [SerializeField] private SmartGameObject _tracer_PREFAB;
        [SerializeField] private WeaponLocalReference _weaponLocalReference;

        private SmartGameObjectPull _objectPull;
        private bool _isDestroyed;

        public Firearm Firearm => _weaponLocalReference.Value;
        public BulletConfiguration BulletConfiguration => Firearm.BullectConfiguration;

        private void Awake()
        {
            _objectPull = new(_tracer_PREFAB);
        }

        private void OnEnable()
        {
            _weaponLocalReference.Shot += OnShot;
        }   

        private void OnDisable()
        {
            _weaponLocalReference.Shot -= OnShot;
        }

        private void OnDestroy()
        {
            _isDestroyed = true;
            _objectPull.Clear();
        }

        private void OnShot(ShotInfo info) => _ = TracerAnimation(BulletConfiguration, info);

        private async Task TracerAnimation(BulletConfiguration bulletConfiguration, ShotInfo info)
        {
            try
            {
                SmartGameObject instance = _objectPull.Instantiate();
                Vector3 startPosition = info.startPosition;
                Vector3 endPoint = info.endPosition;

                instance.transform.position = startPosition;

                float flyTime = Vector3.Distance(endPoint, startPosition) / bulletConfiguration.Speed;

                for (float time = 0; time < flyTime; time += Time.deltaTime)
                {
                    float t = time/flyTime;
                    Vector3 position = Vector3.Lerp(startPosition, endPoint, t);
                    instance.transform.position = position;
                    await Awaitable.NextFrameAsync();
                    if (_isDestroyed)
                    {
                        return;
                    }
                }

                instance.BackToThePull();
                return;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
