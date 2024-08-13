using System;
using System.Collections;
using System.Collections.Generic;
using Alternova.Runtime.Events;
using Payosky.Core.PlainEvent;
using UnityEngine;
using UnityEngine.UI;

namespace Alternova.Runtime.Tiles
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class BoardManager : MonoBehaviour
    {

        [Header("Components")]
        [SerializeField] private Transform tileContainer;
        [SerializeField] private Tile tilePrefab;
        [SerializeField] private GridLayoutGroup grildLayout;

        [Space(10)]
        [Header("Parameters")]
        [SerializeField] private float initialRevealDuration = 3f;

        [SerializeField]
        [Range(2, 4)]
        [Tooltip("What is the amount of cards that need to match")]
        private int groupSize = 2;


        [Space(10)]
        [Header("Conditionals")]
        [SerializeField] int minValue = 0;
        [SerializeField] int maxValue = 9;

        [Space(5)]
        [SerializeField] int minColumns = 2;
        [SerializeField] int maxColumns = 8;

        [Space(5)]
        [SerializeField] int minRows = 2;
        [SerializeField] int maxRows = 8;

        private void OnValidate()
        {
            if (!tileContainer) tileContainer = transform;
            if (!grildLayout) TryGetComponent(out grildLayout);
            grildLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;

        }//Closes OnValidate method

        public void InitializeBoard(TileData[] dataArray)
        {

            if (!IsDataValid(dataArray)) return;

            dataArray = OrganizeTiles(dataArray);


            foreach (var data in dataArray)
            {
                CreateTile(data);
            }

        }//Closes InitializeBoard method

        private Tile CreateTile(TileData data)
        {

            if (data == null || !tilePrefab) return null;

            Tile tile = Instantiate(tilePrefab, tileContainer);
            tile.Initialize(data);
            return tile;

        }//Closes CreateTile method

        public void ClearBoard()
        {

            PlainEventManager.Instance.TriggerEvent(new ClearBoardEvent());

        }//Closes ClearBoard method


        public IEnumerator CO_RevealCards()
        {
            yield return CO_RevealCards(initialRevealDuration);
        }//Closes InitializeBoard Coroutine


        public IEnumerator CO_RevealCards(float duration)
        {

            PlainEventManager.Instance.TriggerEvent(new RevealCardsEvent());
            yield return new WaitForSeconds(duration);
            PlainEventManager.Instance.TriggerEvent(new CoverBoardEvent());

        }//Closes InitializeBoard Coroutine


        private TileData[] OrganizeTiles(TileData[] dataArray)
        {
            int highestC = int.MinValue;

            Array.Sort(dataArray, (x, y) =>
               {
                   int comparison = x.R.CompareTo(y.R);

                   if (comparison == 0) comparison = x.C.CompareTo(y.C);

                   if (x.C > highestC) highestC = x.C;
                   if (y.C > highestC) highestC = y.C;

                   return comparison;
               });

            grildLayout.constraintCount = highestC;
            return dataArray;
        }//Closes SortTiles method


        private bool IsDataValid(TileData[] dataArray)
        {
            Dictionary<int, int> groups = new Dictionary<int, int>();

            if (dataArray == null || dataArray.Length == 0) return false;

            foreach (var data in dataArray)
            {
                if (!IsDataValid(data)) return false;

                if (!groups.ContainsKey(data.number)) groups.Add(data.number, 1);
                else groups[data.number] = groups[data.number] + 1;

            }

            //Calculate if every tile has a valid group to be paired with
            foreach (int group in groups.Values)
                if (group != groupSize) return false;

            return true;
        }//Closes IsDataValid method


        private bool IsDataValid(TileData data)
        {
            if (data == null) return false;
            if (data.number < minValue || data.number > maxValue) return false;
            if (data.R + 1 < minRows || data.R + 1 > maxRows) return false;
            if (data.C + 1 < minColumns || data.C + 1 > maxColumns) return false;
            return true;
        }//Closes IsDataValid method


    }//Closes BoardManager class
}//Closes Namespace declaration
