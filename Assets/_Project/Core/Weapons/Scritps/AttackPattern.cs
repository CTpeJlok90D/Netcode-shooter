using System;
using Unity.Netcode;
using UnityEngine;

namespace Core.Weapons
{
    //[CreateAssetMenu(fileName = "New attack pattern", menuName = "Game/Weapons/Attack pattern")]
    [Icon("Assets/_Project/Core/Weapons/Editor/Icons/icons8-firing-gun-96.png")]
    public abstract class FirearmAttackPattern : ScriptableObject
    {
        protected Firearm Firearm { get; private set; }
        public abstract bool CanAttack { get; }
        public abstract event Action Attacked;
        public abstract event Action DryFire;
        public Bullet Bullet { get; protected set; }
        public bool IsServer => NetworkManager.Singleton.IsServer;
        public FirearmAttackPattern Create(Firearm weapon)
        {
            return Instantiate(this).Init(weapon);
        }

        private FirearmAttackPattern Init(Firearm weapon)
        {
            Firearm = weapon;
            AfterInit();
            return this;
        }

        public virtual void AfterInit(){}
        public virtual void OnAttackStart(){}
        public virtual void OnAttackStop(){}
    }
}