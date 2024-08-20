using UnityEngine;
using Zenject;

namespace Core.PlayerSpawning
{
    public class PlayerSpawnerInstaller : MonoInstaller
    {
        [SerializeField] private PlayerCharacterSpawner _playerSpawner;
        public override void InstallBindings()
        {
            Container
                .Bind<PlayerCharacterSpawner>()
                .FromComponentOn(_playerSpawner.gameObject)
                .AsSingle();
        }
    }
}
