using System.Collections;
using System.Collections.Generic;
using Payosky.Core.Singleton;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace Alternova.Runtime
{
    public sealed class PlayerState : PlainSingleton<PlayerState>
    {
        string userName = "AAA";
        public string Username => userName;
        int totalClicks = 0;
        public int TotalClicks => totalClicks;
        int pairs = 0;
        public int Pairs => pairs;

        int score = 500;
        public int Score => score;


        public void SetUsername(string userName)
        {
            this.userName = userName;
        }

        public void AddClick()
        {
            totalClicks += 1;
        }//Closes Reset method
        public void AddPair()
        {
            pairs += 1;
        }//Closes Reset method

        public void AddScore(int scoreModifier)
        {
            score += scoreModifier;
            if (score < 0) score = 0;

        }//Closes Reset method

        public void Reset()
        {
            totalClicks = 0;
            pairs = 0;
            score = 0;
        }//Closes Reset method


    }//Closes PlayerState class
}//Closes namespace declaration
