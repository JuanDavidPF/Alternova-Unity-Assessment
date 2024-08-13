using UnityEngine;
using System.IO;
using Payosky.Core.Singleton;

namespace Alternova.Runtime.Leaderboards
{

    public class LeaderboardManager : PlainSingleton<LeaderboardManager>
    {
        private const string FileName = "Leaderboard.json";
        private Leaderboard leaderboard = new();
        public Leaderboard Leaderboard => leaderboard;


        public void SaveScore(string playerName, int score)
        {
            LoadLeaderboard();

            LeaderboardEntry newEntry = new LeaderboardEntry(playerName, score);

            leaderboard.entries.Add(newEntry);
            leaderboard.entries.Sort((a, b) => b.score.CompareTo(a.score)); // Sort descending by score

            if (leaderboard.entries.Count > 10)
            {
                leaderboard.entries = leaderboard.entries.GetRange(0, 10);
            }


            SaveLeaderboard();
        }

        public void LoadLeaderboard()
        {
            string filePath = Path.Combine(Application.persistentDataPath, FileName);

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                leaderboard = JsonUtility.FromJson<Leaderboard>(json);
            }
            else
            {
                leaderboard = new Leaderboard();
            }

        }

        private void SaveLeaderboard()
        {
            string filePath = Path.Combine(Application.persistentDataPath, FileName);
            string json = JsonUtility.ToJson(leaderboard, true);
            File.WriteAllText(filePath, json);
        }
    }//Closes LeaderboardManager class
}//Closes Namespac declaration