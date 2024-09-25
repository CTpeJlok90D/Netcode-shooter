using UnityEngine;
using Zenject;

namespace UI.PlayerSpawning
{
    public class LocalSpawnConfigurationContainerInstaller : MonoInstaller
    {
        [SerializeField] private SpawnConfigurationContainer spawnConfigurationContainer = new();
        public override void InstallBindings()
        {
            Container.Bind<SpawnConfigurationContainer>().FromInstance(spawnConfigurationContainer).AsSingle();
        }
    }
}
