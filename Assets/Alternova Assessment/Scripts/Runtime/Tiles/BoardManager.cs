using System;
using System.Collections;
using System.Collections.Generic;
using Alternova.Runtime.Events;
using Payosky.Core.PlainEvent;
using UnityEngine;
using UnityEngine.AI;
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
        [SerializeField] private Image progress;


        private BoardData data;
        private List<Tile> tiles = new();
        private List<Tile> tilesRevealed = new();
        private List<Tile> tilesCleared = new();



        #region Lifecycle
        private void OnValidate()
        {
            if (!tileContainer) tileContainer = transform;
            if (!grildLayout) TryGetComponent(out grildLayout);
            grildLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;

        }//Closes OnValidate method


        #endregion


        #region Event Handlers

        private void OnCardRevealed(CardFlippedEvent e)
        {
            if (!e.Card) return;
            PlayerState.Instance.AddClick();
            tilesRevealed.Add(e.Card);

            if (AreTilesGrouped(tilesRevealed.ToArray()))
            {
                if (data.GroupSize == tilesRevealed.Count)
                {
                    tilesRevealed.Clear();
                    PlainEventManager.TriggerEvent(new ClearGroupedTiles());
                    PlayerState.Instance.AddPair();
                    PlayerState.Instance.AddScore(data.PairScore);
                }
            }
            else
            {
                tilesRevealed.Clear();
                PlayerState.Instance.AddScore(data.WrongPairScore);
                PlainEventManager.TriggerEvent(new CoverRevealedTilesEvent());
            }

        }//Closes OnCardRevealed event

        private void OnCardCleared(CardClearedEvent e)
        {
            if (!e.Card) return;
            if (!tilesCleared.Contains(e.Card)) tilesCleared.Add(e.Card);

            if (tilesCleared.Count == tiles.Count)
            {
                PlainEventManager.TriggerEvent(new GameOverEvent());
            }

        }//Closes OnCardCleared event

        #endregion


        public bool AreTilesGrouped(Tile[] tiles)
        {
            int matchingNumber = 0;
            bool isFirstTile = true;

            foreach (var tile in tiles)
            {
                if (!tile || tile.Data == null) return false;

                if (!isFirstTile && tile.Data.number != matchingNumber) return false;
                else if (isFirstTile)
                {
                    isFirstTile = false;
                    matchingNumber = tile.Data.number;
                }
            }
            return true;
        }//Closes AreTilesGrouped event


        public void InitializeBoard(BoardData data)
        {
            EmptyBoard();
            if (!data) return;
            this.data = data;

            TileData[] dataArray = data.LoadData();

            if (!data.IsDataValid(dataArray)) return;

            dataArray = OrganizeTiles(dataArray);

            foreach (var tileData in dataArray)
            {
                if (tileData == null) continue;
                tiles.Add(CreateTile(tileData));
            }

            StartCoroutine(CO_RevealCards());

        }//Closes InitializeBoard method

        private Tile CreateTile(TileData data)
        {

            if (data == null || !tilePrefab) return null;

            Tile tile = Instantiate(tilePrefab, tileContainer);
            tile.Initialize(data);
            return tile;

        }//Closes CreateTile method

        private void EmptyBoard()
        {

            tiles.Clear();
            tilesCleared.Clear();
            tilesRevealed.Clear();
            PlainEventManager.RemoveEventListener<CardFlippedEvent>(OnCardRevealed);
            PlainEventManager.RemoveEventListener<CardClearedEvent>(OnCardCleared);
            PlainEventManager.TriggerEvent(new EmptyBoardEvent());

        }//Closes ClearBoard method



        public IEnumerator CO_RevealCards()
        {
            yield return new WaitForEndOfFrame();
            yield return CO_RevealCards(data.InitialRevealDuration);
        }//Closes InitializeBoard Coroutine


        public IEnumerator CO_RevealCards(float duration)
        {

            PlainEventManager.TriggerEvent(new RevealCardsEvent());

            StartCoroutine(CO_AnimateProgressBar(duration));
            yield return new WaitForSeconds(duration);

            GameState.Instance.ResetClock();
            PlainEventManager.TriggerEvent(new CoverBoardEvent());


            PlainEventManager.AddEventListener<CardFlippedEvent>(OnCardRevealed);

            PlainEventManager.AddEventListener<CardClearedEvent>(OnCardCleared);


        }//Closes InitializeBoard Coroutine

        private IEnumerator CO_AnimateProgressBar(float duration)
        {
            if (!progress) yield return null;

            float elapsedTime = 1f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float fillAmount = Mathf.Lerp(1f, 0f, elapsedTime / duration);
                progress.fillAmount = fillAmount;

                yield return null; // Wait for the next frame
            }

            progress.fillAmount = 0f; // Ensure it's fully filled at the end
        }


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

    }//Closes BoardManager class
}//Closes Namespace declaration
