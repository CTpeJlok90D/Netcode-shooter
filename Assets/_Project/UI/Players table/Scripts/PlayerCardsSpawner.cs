using Core.Players;
using Core.PlayersScore;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace UI.PlayersTable
{
    public class PlayerCardsSpawner : MonoBehaviour
    {
        [SerializeReference] private PlayerScoreContainer _playerCard_PREFAB;
        [SerializeReference] private Transform _cardsParent;
        [Inject] private PlayersScoreTable _scoreTable;

        private Dictionary<ulong, PlayerScoreContainer> _cardsForPlayers = new();

        private void OnEnable()
        {
            Player.Join += OnPlayerJoin;
            Player.Left += OnPlayerLeft;
        }

        private void OnDisable()
        {
            Player.Join -= OnPlayerJoin;
            Player.Left -= OnPlayerLeft;
        }

        private void OnPlayerJoin(Player player)
        {
            PlayerScoreContainer instance = Instantiate(_playerCard_PREFAB, _cardsParent).Init(player.OwnerClientId, _scoreTable);
            _cardsForPlayers.Add(player.OwnerClientId, instance);
        }

        private void OnPlayerLeft(Player player)
        {
            Destroy(_cardsForPlayers[player.OwnerClientId].gameObject);
            _cardsForPlayers.Remove(player.OwnerClientId);
        }
    }
}
