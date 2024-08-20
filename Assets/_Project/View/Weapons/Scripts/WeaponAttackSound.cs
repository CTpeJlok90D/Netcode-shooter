using Core.Weapons;
using UnityEngine;
using UnityEngine.Audio;

namespace View.Items
{
    public class WeaponAttackSound : MonoBehaviour
    {
        [field: SerializeField] public WeaponLocalReference WeaponReference { get; private set; }
        [field: SerializeField] public AudioResource AudioResource { get; private set; }
        [field: SerializeField] public float SoundLenght {get; private set;} = 3;
        [field: SerializeField] public AudioPull AudioSourcePull { get; private set; }


        private void OnEnable()
        {
            WeaponReference.Attacked += OnWeaponAttack;
        }

        private void OnDisable()
        {
            WeaponReference.Attacked -= OnWeaponAttack;
        }

        private void OnWeaponAttack()
        {
            _ = AudioSourcePull.Play(AudioResource, SoundLenght);
        }
    }
}
