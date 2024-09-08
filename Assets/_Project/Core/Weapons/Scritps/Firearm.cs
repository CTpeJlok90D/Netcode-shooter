using Core.Items;
using Unity.Netcode;
using UnityEngine;
using Core.Characters;
using System;

namespace Core.Weapons
{
    [RequireComponent(typeof(Useble))]
    [RequireComponent(typeof(DestroyEvent))]
    public sealed class Firearm : NetworkBehaviour
    {
        [field: SerializeField] public DestroyEvent DestroyEvent { get; private set; }
        [field: SerializeField] public Useble Useble { get; private set; }
        [field: SerializeField] public FirearmAttackPattern AttackPattern { get; private set; }
        [field: SerializeField] public Transform AttackPoint { get; private set; }
        [field: SerializeField] public float RateOfFire { get; private set; } = 3;
        [field: SerializeField] public BulletConfiguration BullectConfiguration { get; private set; }
        [field: SerializeField] public float MaxSpeedToAttack { get; private set; } = 3.5f;


        private TopdownCharacter _topdownCharacter;
        public TopdownCharacter TopdownCharacter 
        {
            get
            {
                ValidateTopdownCharacter();
                return _topdownCharacter;
            }
        }

        public event Action Attacked
        {
            add => AttackPattern.Attacked += value;
            remove => AttackPattern.Attacked -= value;
        }

        public GameObject Owner => Useble.Owner;

        private void Awake()
        {
            AttackPattern = AttackPattern.Create(this);
        }
    
        private void ValidateTopdownCharacter()
        {
            if (_topdownCharacter != null)
            {
                return;
            }
            if (Owner != null && Owner.TryGetComponent(out TopdownCharacter topdownCharacter))
            {
                _topdownCharacter = topdownCharacter;
            }
        }

        private void Update()
        {
            ValidateAttack();
        }

        private void ValidateAttack() 
        {
            if (AttackPattern == null || TopdownCharacter == null) 
            {
                return;
            }
            if ((AttackPattern.CanAttack == false || TopdownCharacter.Speed > MaxSpeedToAttack) && Useble.IsUsing)
            {
                StopAttack();
            }
        }

        private void OnEnable()
        {
            Useble.UsageHasStarted += OnUsageStart;
            Useble.UsageHasEnded += OnUsageEnd;
        }

        private void OnDisable()
        {
            Useble.UsageHasStarted -= OnUsageStart;
            Useble.UsageHasEnded -= OnUsageEnd;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Destroy(AttackPattern);
        }

        private void OnUsageStart() => StartAttack();
        private void StartAttack()
        {
            if (TopdownCharacter.Speed > MaxSpeedToAttack)
            {
                return;
            }
            
            AttackPattern.OnAttackStart();
        }

        private void OnUsageEnd() => StopAttack();
        private void StopAttack()
        {
            AttackPattern.OnAttackStop();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            DestroyEvent ??= GetComponent<DestroyEvent>();
        }
#endif
    }
}