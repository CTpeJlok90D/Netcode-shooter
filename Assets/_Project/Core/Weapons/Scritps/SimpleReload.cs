using System;
using System.Threading.Tasks;
using Core.Characters;
using Unity.Netcode;
using UnityEngine;

namespace Core.Weapons
{
    [RequireComponent(typeof(Clip))]
    public class SimpleReload : Reloadeble
    {
        [field: SerializeField] public Clip Clip { get; private set; }
        [field: SerializeField] public float ReloadTime { get; private set; }
        [field: SerializeField] public Firearm Firearm { get; private set; }
        [field: SerializeField] public SpeedModificator ReloadSpeedModifier { get; private set; } = new() 
        {
            AccelerationClamp = 2,
            MaxSpeedClamp = 1.5f,
        };

        public bool IsReloading { get; protected set; } = false;
        public TopdownCharacter Character => Firearm.TopdownCharacter;

        public override bool CanReload => Clip.Ammo.Value < Clip.MaxAmmo && IsReloading == false;
        public override bool NeedReload => Clip.Ammo.Value == 0;

        public override event ReloadStartedListener ReloadStarted;
        public override event ReloadStartedListener ReloadCompleted;

        protected bool IsDestroyed;
        protected bool IsReloadIsBroked;

        public override void Reload()
        {
            ReloadRPC();
        }

        protected virtual void OnEnable() 
        {
            Firearm.Attacked += OnAttacked;
        }
        protected virtual void OnDisable() 
        {
            Firearm.Attacked -= OnAttacked;
        }

        private void OnAttacked()
        {
            BrokeReload();
        }

        [Rpc(SendTo.Everyone)]
        private void ReloadRPC()
        {
            if (CanReload == false)
            {
                return;
            }
            _ = ReloadTask();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            IsDestroyed = true;
        }

        protected virtual async Task ReloadTask()
        {
            try
            {
                if (IsOwner)
                {
                    Clip.Ammo.Value = 0;
                    Character.SpeedModificators.Add(ReloadSpeedModifier);
                }

                ReloadStarted?.Invoke();
                IsReloading = true;
                
                await Awaitable.WaitForSecondsAsync(ReloadTime);
                if (IsDestroyed || IsReloadIsBroked) 
                {
                    IsReloadIsBroked = false;
                    return; 
                }

                if (IsOwner)
                {
                    Character.SpeedModificators.Remove(ReloadSpeedModifier);
                    Clip.Ammo.Value = Clip.MaxAmmo;
                }
               
                ReloadCompleted?.Invoke();
                IsReloading = false;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public override void BrokeReload()
        {
            if (IsReloading) 
            {
                Character.SpeedModificators.Remove(ReloadSpeedModifier);
                IsReloading = false;
                IsReloadIsBroked = true;
            }
        }
    }
}
