using System;
using System.Threading.Tasks;
using Core.Weapons;
using Unity.Netcode;
using UnityEngine;

namespace Data.Weapons
{
    [CreateAssetMenu(fileName = "New automatic attack pattern", menuName = "Game/Weapons/Attack patterns/Automatic")]
    public class AutomaticAttackPattern : FirearmAttackPattern
    {
        private Reloadeble _reloadeble;
        private bool _isAttacking;
        private bool _attackDelay;

        public override bool CanAttack => _reloadeble == null || _reloadeble.NeedReload == false;
        public override event Action Attacked;
        public override event Action DryFire;

        public override void AfterInit()
        {
            _reloadeble = Firearm.GetComponent<Reloadeble>();
            Bullet = new(Firearm.BullectConfiguration);
        }

        public override void OnAttackStart()
        {
            if (_reloadeble != null && _reloadeble.NeedReload)
            {
                DryFire?.Invoke();
                return;
            }

            _isAttacking = true;
            _ = Attack();
        }

        private async Task Attack()
        {
            try
            {
                float delayBetweenShots = 1 / Firearm.RateOfFire;

                while (_attackDelay)
                {
                    await Awaitable.NextFrameAsync();
                }

                while (_isAttacking)
                {
                    _ = Bullet.Shoot(Firearm.TopdownCharacter.gameObject, Firearm.TopdownCharacter.Velocity.magnitude, Firearm.TopdownCharacter.LookPoint);
                    Attacked?.Invoke();

                    _attackDelay = true;
                    await Awaitable.WaitForSecondsAsync(delayBetweenShots);
                    if (NetworkManager.Singleton.IsConnectedClient == false)
                    {
                        return;
                    }
                    _attackDelay = false;

                    if (CanAttack == false)
                    {
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public override void OnAttackStop()
        {
            _isAttacking = false;
        }

        private void OnDestroy()
        {
            _isAttacking = false;
        }
    }
}
