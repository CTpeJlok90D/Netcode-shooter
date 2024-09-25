using Core.Players;
using Unity.Netcode;

namespace Core.PlayersScore
{
    public class PlayersScoreTable : NetworkBehaviour
    {
        public NetworkList<PlayerScore> Scores { get; private set; }

        private void Awake()
        {
            Scores = new();
        }

        private void OnEnable()
        {
            Player.Join -= OnPlayerJoin;
            Player.Left -= OnPlayerLeft;
        }

        private void OnDisable()
        {
            Player.Join -= OnPlayerJoin;
            Player.Left -= OnPlayerLeft;
        }

        private void OnPlayerJoin(Player player)
        {
            if (NetworkManager.IsServer == false) 
            {
                return;
            }

            PlayerScore playerScore = new()
            {
                PlayerID = player.OwnerClientId,
                PlayerObject = player.NetworkObject
            };

            Scores.Add(playerScore);
        }

        private void OnPlayerLeft(Player player)
        {
            if (NetworkManager.IsServer == false)
            {
                return;
            }

            int index = 0;
            foreach (PlayerScore score in Scores) 
            {
                if (score.PlayerID == player.OwnerClientId) 
                {
                    index = Scores.IndexOf(score);
                }
            }

            Scores.RemoveAt(index);
        }
    }
}