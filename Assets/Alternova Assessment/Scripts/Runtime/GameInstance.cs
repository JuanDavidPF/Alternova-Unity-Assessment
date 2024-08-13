using System;
using System.IO;
using Alternova.Runtime.Events;
using Alternova.Runtime.Leaderboards;
using Alternova.Runtime.Tiles;
using Payosky.Core.PlainEvent;
using UnityEngine;

namespace Alternova.Runtime
{
    public class GameInstance : MonoBehaviour
    {

        [SerializeField] private BoardManager board;
        [SerializeField] private BoardData boardData;

        private void Start()
        {
            LeaderboardManager.Instance.LoadLeaderboard();
        }//Closes Start method

        public void StartBoard()
        {
            PlayerState.Instance.Reset();


            if (board && boardData)
            {
                board.InitializeBoard(boardData);
                PlainEventManager.AddEventListener<GameOverEvent>(OnGameOverEvent);
            }

        }//Closes StartBoard method
        public void SaveGameSession()
        {
            GameSessionStatsWrapper result = new(
              new(PlayerState.Instance.TotalClicks,
                    GameState.Instance.time,
                    PlayerState.Instance.Pairs,
                    PlayerState.Instance.Score
            )
            );


            string jsonResult = JsonUtility.ToJson(result, true);
            string dateTime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

            string filePath = Path.Combine(Application.persistentDataPath, $"GameSession_{dateTime}.json");
            File.WriteAllText(filePath, jsonResult);

        }//Closes StartBoard method

        public void SaveToLeaderboard()
        {
            LeaderboardManager.Instance.SaveScore(PlayerState.Instance.Username, PlayerState.Instance.Score);

        }//Closes SaveToLeaderboard method


        public void OnGameOverEvent(GameOverEvent e)
        {
            PlainEventManager.RemoveEventListener<GameOverEvent>(OnGameOverEvent);
            GameState.Instance.StopClock();
            SaveGameSession();
            SaveToLeaderboard();
        }//Closes OnGameOverEvent method

        private void Update()
        {
            GameState.Instance.UpdateClock(Time.deltaTime);

        }//Closes Update method

        private void OnDestroy()
        {
            PlainEventManager.RemoveAllEvents();
        }//Closes OnDestroy method


    }//Closes GameInstance class
}//Closes Namespace declaration
