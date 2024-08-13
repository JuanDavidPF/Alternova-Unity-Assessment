using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alternova.Runtime.Leaderboards
{

    [System.Serializable]
    public class LeaderboardEntry
    {
        public string playerName;
        public int score;

        public LeaderboardEntry(string playerName, int score)
        {
            this.playerName = playerName;
            this.score = score;
        }
    }

    [System.Serializable]
    public class Leaderboard
    {
        public List<LeaderboardEntry> entries = new();
    }


}
