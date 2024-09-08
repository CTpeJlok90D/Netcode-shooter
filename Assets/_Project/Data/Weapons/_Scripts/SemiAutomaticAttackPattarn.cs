using System;
using System.Threading.Tasks;
using Core.Weapons;
using Unity.Netcode;
using UnityEngine;

namespace Data.Weapons
{
    [CreateAssetMenu(fileName = "New semi automatic attack pattern", menuName = "Game/Weapons/Attack patterns/Semi automatic")]
    public class SemiAutomaticAttackPattern : FirearmAttackPattern
    {
        private bool _isStoppedByDelay;
        private Reloadeble _reloadeble;

        public override bool CanAttack => (_reloadeble == null || _reloadeble.NeedReload == false) && _isStoppedByDelay == false;
        public override event Action Attacked;
        public override event Action DryFire;

        public override void AfterInit()
        {
            _reloadeble = Firearm.GetComponent<Reloadeble>();
            Bullet = new(Firearm.BullectConfiguration);
        }

        public override void OnAttackStart()
        {
            if (CanAttack == false)
            {
                if (_reloadeble != null && _reloadeble.NeedReload) 
                {
                    DryFire?.Invoke();
                }
                return;
            }

            _ = Bullet.Shoot(Firearm.TopdownCharacter.gameObject, Firearm.TopdownCharacter.Velocity.magnitude, Firearm.TopdownCharacter.LookPoint);
            Attacked?.Invoke();
            _ = DelayBetweenAttacks();
        }

        private async Task DelayBetweenAttacks()
        {
            try 
            {
                _isStoppedByDelay = true;
                float delay = 1 / Firearm.RateOfFire;
                await Awaitable.WaitForSecondsAsync(delay);
                _isStoppedByDelay = false;
            }
            catch (Exception e) 
            {
                Debug.LogException(e);
            }
        }
    }
}