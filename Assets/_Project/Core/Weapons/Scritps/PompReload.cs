using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Weapons
{
    public class PompReload : SimpleReload
    {
        public delegate void BulletInjectedListener();

        public override event ReloadStartedListener ReloadStarted;
        public override event ReloadStartedListener ReloadCompleted;
        public event BulletInjectedListener BulletInjected;

        protected override async Task ReloadTask()
        {
            try 
            {
                if (IsOwner)
                {
                    Character.SpeedModificators.Add(ReloadSpeedModifier);
                }

                ReloadStarted?.Invoke();
                IsReloading = true;

                while (Clip.Ammo.Value < Clip.MaxAmmo) 
                {
                    await Awaitable.WaitForSecondsAsync(ReloadTime);
                    if (IsDestroyed || IsReloadIsBroked)
                    {
                        IsReloadIsBroked = false;
                        return;
                    }

                    if (IsOwner)
                    {
                        Clip.Ammo.Value++;
                    }
                    BulletInjected?.Invoke();
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
    }
}
