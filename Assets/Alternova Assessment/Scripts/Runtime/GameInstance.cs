using System.Collections;
using System.Collections.Generic;
using Alternova.Runtime.Events;
using Alternova.Runtime.Tiles;
using Payosky.Core.PlainEvent;
using UnityEngine;

namespace Alternova.Runtime
{
    public class GameInstance : MonoBehaviour
    {

        [SerializeField] private BoardManager board;
        [SerializeField] private BoardData boardData;

        public void StartBoard()
        {
            PlayerState.Instance.Reset();


            if (board && boardData)
            {
                board.InitializeBoard(boardData);
                PlainEventManager.AddEventListener<GameOverEvent>(OnGameOverEvent);
            }

        }//Closes StartBoard method


        public void OnGameOverEvent(GameOverEvent e)
        {
            PlainEventManager.RemoveEventListener<GameOverEvent>(OnGameOverEvent);
            GameState.Instance.StopClock();
        }

        private void Update()
        {
            GameState.Instance.UpdateClock(Time.deltaTime);

        }//Closes Update method

        private void OnDestroy()
        {
            PlainEventManager.RemoveAllEvents();
        }

    }//Closes GameInstance class
}//Closes Namespace declaration
