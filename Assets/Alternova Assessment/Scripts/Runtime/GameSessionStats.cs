
using UnityEngine;

namespace Alternova.Runtime
{
    [System.Serializable]
    public struct GameSessionStats
    {
        public int total_clicks;
        public int total_time;
        public int pairs;
        public int score;

        public GameSessionStats(int total_clicks, int total_time, int pairs, int score)
        {
            this.total_clicks = total_clicks;
            this.total_time = total_time;
            this.pairs = pairs;
            this.score = score;
        }
    }

    [System.Serializable]
    public class GameSessionStatsWrapper
    {

        public GameSessionStats result;
        public GameSessionStatsWrapper(GameSessionStats result)
        {
            this.result = result;
        }
    }
}
