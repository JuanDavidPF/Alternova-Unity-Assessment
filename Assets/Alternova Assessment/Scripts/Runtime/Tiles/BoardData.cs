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
        public float initialRevealDuration = 3f;

        [Range(2, 4)]
        [Tooltip("What is the amount of cards that need to match")]
        public int groupSize = 2;

        [Space(10)]
        [Header("Score")]
        [Range(10, 300)]
        public int pairScore = 100;
        [Range(-100, -5)] public int wrongPairScore = -20;


        [Space(10)]
        [Header("Conditionals")]
        public int minValue = 0;
        public int maxValue = 9;

        [Space(5)]
        public int minColumns = 2;
        public int maxColumns = 8;

        [Space(5)]
        public int minRows = 2;
        public int maxRows = 8;



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
            if (dataArray == null || dataArray.Length == 0) return false;

            Dictionary<int, int> groups = new();
            int highestRow = int.MinValue;
            int highestCol = int.MinValue;

            foreach (var data in dataArray)
            {
                if (data == null) return false;
                if (data.number < minValue || data.number > maxValue) return false;

                if (data.R > highestRow) highestRow = data.R;
                if (data.C > highestCol) highestCol = data.C;

                if (!groups.ContainsKey(data.number)) groups.Add(data.number, 1);
                else groups[data.number] = groups[data.number] + 1;

            }

            //Check if the tile position is inside the board bounds
            if (highestRow < minRows || highestRow > maxRows) return false;
            if (highestCol < minColumns || highestCol > maxColumns) return false;


            //Calculate if every tile has a valid group to be paired with
            foreach (int group in groups.Values)
                if (group != groupSize) return false;

            return true;
        }//Closes IsDataValid method




    }//Closes JsonBoard scriptableobject
}//Closes namespace declaration