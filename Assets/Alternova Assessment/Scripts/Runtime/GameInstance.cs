using System.Collections;
using System.Collections.Generic;
using Alternova.Runtime.Tiles;
using UnityEngine;

namespace Alternova.Runtime
{
    public class GameInstance : MonoBehaviour
    {

        [SerializeField] private BoardManager board;
        [SerializeField] private JsonDataLoader jsonLoader;


        private IEnumerator Start()
        {
            if (board)
            {
                board.InitializeBoard(LoadTileData(jsonLoader));
                yield return new WaitForSeconds(3f);
                StartCoroutine(board.CO_RevealCards());
            }

        }//Closes Start method


        public TileData[] LoadTileData(JsonDataLoader jsonLoader)
        {
            if (!jsonLoader) return new TileData[0];

            TileDataWrapper result = jsonLoader.LoadData<TileDataWrapper>();

            if (result != null) return result.blocks;
            else return new TileData[0];

        }//Closes LoadTileData method




    }//Closes GameInstance class
}//Closes Namespace declaration
