using Core.PlayerSpawning;
using UnityEngine;
using Zenject;

namespace Core.Characters
{
    public class HaveCharacterObject : MonoBehaviour
    {
        [field: SerializeField] public GameObject Target { get; private set; }
        [field: SerializeField] public bool Inverse { get; private set; }
        [Inject] public PlayerCharacterSpawner PlayerCharacterSpawner { get; private set; }

        private void Update()
        {
            bool active = PlayerCharacterSpawner.LocalPlayerCharacter != null;

            if (Inverse) 
            {
                active = !active;
            }
            Target.SetActive(active);
        }
    }
}