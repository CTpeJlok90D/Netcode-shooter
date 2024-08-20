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
        private BulletConfiguration _configuration;
        public delegate void ShotListener(ShotInfo info);
        public event ShotListener Shot;

        public Bullet(BulletConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Shoot(TopdownCharacter sender)
        {
            try
            {
                float spreadDistance = _configuration.SpreadPerSpeed.Evaluate(sender.Velocity.magnitude);
                float spreadRange = spreadDistance/2;
                Vector3 spreadOffcet = new Vector3(UnityEngine.Random.Range(-spreadRange, spreadRange), UnityEngine.Random.Range(-spreadRange, spreadRange), UnityEngine.Random.Range(-spreadRange, spreadRange));

                Transform shotTransform = _configuration.ShotPoint;
                Vector3 endPoint = sender.LookPoint + spreadOffcet;
                Vector3 direction = (endPoint - shotTransform.position).normalized;
                Ray ray = new(shotTransform.position, direction);

                ShotInfo shotInfo = new()
                {
                    startPosition = shotTransform.position,
                    endPosition = endPoint,
                    hittenObject = null
                };

                if (Physics.Raycast(ray, out RaycastHit hitInfo , _configuration.MaxDistance))
                {
                    GameObject hittenObject = hitInfo.collider.gameObject;         
                    shotInfo.hittenObject = hittenObject;
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
    }

    public struct ShotInfo
    {
        public Vector3 startPosition;
        public Vector3 endPosition;
        public GameObject hittenObject;
    }
}