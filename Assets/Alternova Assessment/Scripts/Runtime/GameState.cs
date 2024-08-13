using System.Collections;
using System.Collections.Generic;
using Payosky.Core.Singleton;
using UnityEngine;

namespace Alternova.Runtime
{
    public sealed class GameState : PlainSingleton<GameState>
    {
        private float elapsedTime;
        private bool isRunning;

        public void StartClock()
        {
            isRunning = true;
        }//Closes StartClock method

        // Stop the clock
        public void StopClock()
        {
            isRunning = false;

        }//Closes StopClock method


        public void ResetClock()
        {
            elapsedTime = 0;
            StartClock();
        }//Closes ResetClock method


        public void UpdateClock(float deltaTime)
        {
            if (isRunning) elapsedTime += Time.deltaTime;

        }//Closes UpdateClock method
        public int time => (int)elapsedTime;

        public string GetFormattedTime()
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            return string.Format("{0:D2}:{1:D2}", minutes, seconds);
        }//Closes GetFormattedTime method

    }//Closes CloGameStateck class
}//Closes namespace declaration
