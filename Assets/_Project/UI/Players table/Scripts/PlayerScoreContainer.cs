using Core.PlayersScore;
using Unity.Netcode;
using UnityEngine;

namespace UI.PlayersTable 
{
    public class PlayerScoreContainer : MonoBehaviour
    {
        private PlayersScoreTable _scoreTable;
        private ulong _linkedPlayerID;
        private PlayerScore _score;

        public PlayerScore PlayerScore => _score;
        public delegate void PlayerScoreChanged(PlayerScore score);
        public event PlayerScoreChanged Changed; 

        public PlayerScoreContainer Init(ulong PlayerID, PlayersScoreTable scoreTable)
        {
            _linkedPlayerID = PlayerID;
            _scoreTable = scoreTable;
            return this;
        }

        private void OnEnable()
        {
            _scoreTable.Scores.OnListChanged += OnListChange;
            ValidateValues();
        }

        private void OnDisable()
        {
            _scoreTable.Scores.OnListChanged -= OnListChange;
        }

        private void OnListChange(NetworkListEvent<PlayerScore> changeEvent) => ValidateValues();
        private void ValidateValues() 
        {
            for (int i = 0; i < _scoreTable.Scores.Count; i++)
            {
                if (_linkedPlayerID == _scoreTable.Scores[i].PlayerID)
                {
                    _score = _scoreTable.Scores[i];
                    Changed?.Invoke(_score);
                }
            }
        }
    }
}
