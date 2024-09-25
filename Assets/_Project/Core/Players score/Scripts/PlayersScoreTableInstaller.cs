using UnityEngine;
using Zenject;

namespace Core.PlayersScore
{
    public class PlayersScoreTableInstaller : MonoInstaller
    {
        [SerializeField] private PlayersScoreTable PlayersScoreTable;

        public override void InstallBindings()
        {
            Container.Bind<PlayersScoreTable>().FromInstance(PlayersScoreTable).AsSingle();
        }
    }
}
