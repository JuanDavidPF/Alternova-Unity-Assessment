using System.Collections.Generic;
using Alternova.Runtime.Tiles;
using UnityEngine;


namespace Alternova.Runtime.Tiles
{
    [CreateAssetMenu(fileName = "new Board Data", menuName = "ScriptableObjects/Board Data", order = 1)]

    public class BoardData : ScriptableObject
    {
        [SerializeField] private TextAsset jsonFile;

        [Space(10)]
        [Header("Parameters")]
        [SerializeField] private float initialRevealDuration = 3f;
        public float InitialRevealDuration => initialRevealDuration;

        [SerializeField]
        [Range(2, 4)]
        [Tooltip("What is the amount of cards that need to match")]
        private int groupSize = 2;
        public int GroupSize => groupSize;


        [Space(10)]
        [Header("Score")]
        [SerializeField][Range(10, 300)] int pairScore = 100;
        public int PairScore => pairScore;


        [SerializeField][Range(-100, -5)] int wrongPairScore = -20;
        public int WrongPairScore => wrongPairScore;



        [Space(10)]
        [Header("Conditionals")]
        [SerializeField] int minValue = 0;
        public int MinValue => minValue;

        [SerializeField] int maxValue = 9;
        public int MaxValue => maxValue;


        [Space(5)]
        [SerializeField] int minColumns = 2;
        public int MinColumns => minColumns;

        [SerializeField] int maxColumns = 8;
        public int MaxColumns => maxColumns;


        [Space(5)]
        [SerializeField] int minRows = 2;
        public int MinRows => minRows;

        [SerializeField] int maxRows = 8;
        public int MaxRows => maxRows;


        public TileData[] LoadData()
        {

            if (jsonFile == null) return null;

            try
            {
                TileDataWrapper data = JsonUtility.FromJson<TileDataWrapper>(jsonFile.text);
                return data.blocks;
            }
            catch (System.Exception)
            {
                return null;
            }
        }//Closes LoadData method
        public bool IsDataValid(TileData[] dataArray)
        {
            Dictionary<int, int> groups = new();

            if (dataArray == null || dataArray.Length == 0) return false;

            foreach (var data in dataArray)
            {
                if (!IsDataValid(data)) return false;

                if (!groups.ContainsKey(data.number)) groups.Add(data.number, 1);
                else groups[data.number] = groups[data.number] + 1;

            }

            //Calculate if every tile has a valid group to be paired with
            foreach (int group in groups.Values)
                if (group != GroupSize) return false;

            return true;
        }//Closes IsDataValid method


        public bool IsDataValid(TileData data)
        {
            if (data == null) return false;
            if (data.number < MinValue || data.number > MaxValue) return false;
            if (data.R + 1 < MinRows || data.R + 1 > MaxRows) return false;
            if (data.C + 1 < MinColumns || data.C + 1 > MaxColumns) return false;
            return true;
        }//Closes IsDataValid method


    }//Closes JsonBoard scriptableobject
}//Closes namespace declaration