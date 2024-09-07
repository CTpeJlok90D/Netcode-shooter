using UnityEngine;
using Core.Weapons;
using Core.Characters;

namespace Data.Weapons
{
    [RequireComponent(typeof(Firearm))]
    public class SimpleAim : Aimable
    {
        [field: SerializeField] public Firearm Weapon { get; private set; }
        [field: SerializeField] public SpeedModificator AimedSpeedModifier { get; private set; } = new() 
        {
            AccelerationClamp = 8,
            MaxSpeedClamp = 1.5f,
        };

        public TopdownCharacter Character => Weapon.TopdownCharacter;

        protected override void OnAim()
        {
            if (Character.IsOwner == false)
            {
                return;
            }
            Character.SpeedModificators.Add(AimedSpeedModifier);
        }

        protected override void OnAimStop()
        {                       
            if (Character.IsOwner == false)
            {
                return;
            }
            Character.SpeedModificators.Remove(AimedSpeedModifier);
        }
    }
}