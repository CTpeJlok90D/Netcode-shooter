using System;
using UnityEngine;

namespace Core.Weapons
{
    [Serializable]
    public record BulletConfiguration
    {
        public Transform ShotPoint;
        public float Damage;
        public AnimationCurve DamageMultiplyPerDistance;
        public float MaxDistance;
        public float Speed;
        public AnimationCurve SpreadPerSpeed;
        public AnimationCurve SpreadPerSpray;
        public AnimationCurve SpreadPerDistance;
        public float SpraySpreadFallRate = 2f;
    }
}