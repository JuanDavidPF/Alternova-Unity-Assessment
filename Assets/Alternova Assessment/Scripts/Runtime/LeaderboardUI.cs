using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Alternova.Runtime
{
    public class LeaderboardUI : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI listTMP;


        private void OnEnable()
        {
            RenderList();

        }//Closes OnEnable method


        public void RenderList()
        {
            if (!listTMP) return;


            System.Text.StringBuilder sb = new();
            List<LeaderboardEntry> entries = LeaderboardManager.Instance.Leaderboard.entries;

            for (int i = 0; i < entries.Count; i++)
            {
                LeaderboardEntry entry = entries[i];
                string line = string.Format(
                    "{0,-" + 30 + "} {1,-" + 25 + "} {2," + 5 + "}",
                    i + 1,
                    entry.playerName,
                    entry.score
                );

                sb.AppendLine(line);
            }

            listTMP.text = sb.ToString();
        }//Closes RenderList method



    }//Closes LeaderboardUI class
}//Closes Namespace declaration
