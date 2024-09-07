using Unity.Netcode;
using Unity.Netcode.Custom;
using UnityEngine;

namespace Core.Weapons
{   
    [RequireComponent(typeof(Firearm))]
    public sealed class Clip : NetworkBehaviour
    {
        [field: SerializeField] public Firearm Firearm { get; private set; }
        [field: SerializeField] public NetVariable<int> Ammo { get; private set; }
        [field: SerializeField] public int MaxAmmo { get; private set; } = 30;
        [field: SerializeField] public int AmmoConsumptionPetAttack { get; private set; } = 1;

        public void Awake()
        {
            Ammo = new(MaxAmmo, writePerm: NetworkVariableWritePermission.Owner);
        }

        private void OnEnable()
        {
            Firearm.Attacked += OnAttack;
        }

        private void OnDisable()
        {
            Firearm.Attacked -= OnAttack;            
        }

        private void OnAttack()
        {
            if (IsOwner == false)
            {
                return;
            }
            Ammo.Value -= AmmoConsumptionPetAttack;
        }
    }
}