using Core.Items;
using Unity.Netcode;
using UnityEngine;
using System;
using Core.Weapons;

namespace Core.Character
{
    public class Inventory : NetworkBehaviour
    {
        [field: SerializeField] public UsebleReference Selected { get; private set; }
        [field: SerializeField] public ItemSlot MainWeapon { get; private set; }
        [field: SerializeField] public ItemSlot AddictionalWeapon { get; private set; }

        public void SelectWeapon(Useble weapon) 
        {
            if (weapon == null) 
            {
                throw new ArgumentNullException($"Weapon is null");
            }
            
            if (NetworkManager.IsServer == false && IsOwner == false) 
            {
                return;
            }

            if (Selected.Value != null) 
            {
                if (Selected.Value.TryGetComponent(out Aimable aimable)) 
                {
                    aimable.IsAiming = false;
                }

                if (Selected.Value.TryGetComponent(out Reloadeble reloadeble)) 
                {
                    reloadeble.BrokeReload();
                }
            }

            if (Selected.Value != weapon)
            {
                Selected.Value = weapon;
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (NetworkManager.IsServer == false) 
            {
                return;
            }

            MainWeapon.Item.NetworkObject.Despawn(true);
            AddictionalWeapon.Item.NetworkObject.Despawn(true);
        }
    }
}
