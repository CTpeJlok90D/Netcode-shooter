using System;
using System.Threading.Tasks;
using Core.Characters;
using Core.HealthSystem;
using Unity.Netcode;
using UnityEngine;

namespace Core.Weapons
{
    public class Bullet
    {
        public delegate void ShotListener(ShotInfo info);
        public event ShotListener Shot;

        private BulletConfiguration _configuration;
        private float _bulletNumber;
        private Task _spraySpreadFallRate;
        private bool _isDestroyed;

        public float SprayDistance(float senderSpeed) => _configuration.SpreadPerSpeed.Evaluate(senderSpeed) * _configuration.SpreadPerSpray.Evaluate(_bulletNumber);

        public Bullet(BulletConfiguration configuration)
        {
            _configuration = configuration;
            _ = FallSpreySpread();
        }

        ~Bullet() 
        {
            _isDestroyed = true;
        }

        public async Task Shoot(GameObject sender, float senderSpeed, Vector3 targetPoint)
        {
            try
            {
                float spreadDistance = SprayDistance(senderSpeed);
                float spreadRange = spreadDistance/2;
                Vector3 spreadOffcet = new Vector3(UnityEngine.Random.Range(-spreadRange, spreadRange), UnityEngine.Random.Range(-spreadRange, spreadRange), UnityEngine.Random.Range(-spreadRange, spreadRange));

                Transform shotTransform = _configuration.ShotPoint;
                Vector3 endPoint = targetPoint + spreadOffcet;
                Vector3 direction = (endPoint - shotTransform.position).normalized;
                Ray ray = new(shotTransform.position, direction);

                ShotInfo shotInfo = new()
                {
                    startPosition = shotTransform.position,
                    endPosition = endPoint,
                    hittenObject = null
                };

                _bulletNumber++;

                if (Physics.Raycast(ray, out RaycastHit hitInfo , _configuration.MaxDistance))
                {
                    GameObject hittenObject = hitInfo.collider.gameObject;         
                    shotInfo.hittenObject = hittenObject;
                    shotInfo.endPosition = hitInfo.point;
                    Shot?.Invoke(shotInfo);

                    if (hittenObject.TryGetComponent(out Health health))
                    {
                        float damageDelay = hitInfo.distance / _configuration.Speed;
                        await Awaitable.WaitForSecondsAsync(damageDelay);
                        if (NetworkManager.Singleton.IsConnectedClient == false)
                        {
                            return;
                        }
                        
                        DamageInfo info = new(sender.gameObject, _configuration.Damage);
                        health.DealDamage(info);
                    }
                }
                else
                {
                    Shot?.Invoke(shotInfo);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private async Task FallSpreySpread() 
        {
            while (Application.isPlaying && _isDestroyed == false) 
            {
                _bulletNumber = Mathf.Clamp(_bulletNumber - _configuration.SpraySpreadFallRate * Time.deltaTime, 0, Mathf.Infinity);
                await Awaitable.NextFrameAsync();
            }
        }
    }

    public struct ShotInfo
    {
        public Vector3 startPosition;
        public Vector3 endPosition;
        public GameObject hittenObject;
    }
}