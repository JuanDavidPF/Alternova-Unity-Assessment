using System.Collections;
using Alternova.Runtime.Events;
using CodiceApp;
using Payosky.Core.PlainEvent;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Alternova.Runtime.Tiles
{

    [RequireComponent(typeof(Animator))]
    public sealed class Tile : MonoBehaviour
    {

        [Header("Components")]
        [SerializeField] private TextMeshProUGUI tileLabel;
        [SerializeField] private Image cardBG;
        [SerializeField] private Animator animator;

        [Space(10)]
        [Header("Prameters")]
        [SerializeField] private float flipAnimationDuration = 0.2f;

        private TileData data;
        public TileData Data => data;
        private ETileState tileState = ETileState.LOCKED;


        #region Lifecycle
        private void OnValidate()
        {

            if (!animator) TryGetComponent(out animator);

        }//Closes Awake method


        private void Awake()
        {
            PlainEventManager.AddEventListener<CoverBoardEvent>(OnCoverTile);
            PlainEventManager.AddEventListener<RevealCardsEvent>(OnRevealCardsEvent);
            PlainEventManager.AddEventListener<EmptyBoardEvent>(OnBoardEmptied);

        }//Closes Start method


        public void Initialize(TileData data)
        {
            this.data = data;
            if (tileLabel) tileLabel.text = data.number.ToString();

        }//Closes Initialize method

        private void OnDestroy()
        {
            PlainEventManager.RemoveEventListener<CoverBoardEvent>(OnCoverTile);
            PlainEventManager.RemoveEventListener<RevealCardsEvent>(OnRevealCardsEvent);
            PlainEventManager.RemoveEventListener<EmptyBoardEvent>(OnBoardEmptied);
            PlainEventManager.RemoveEventListener<CoverRevealedTilesEvent>(OnCoverRevealedTile);
            PlainEventManager.RemoveEventListener<ClearGroupedTiles>(OnClearGroupedTiles);
        }
        #endregion


        #region Event Handlers

        private void OnCoverTile(CoverBoardEvent e)
        {

            StartCoroutine(CO_SetTileState(ETileState.COVERED, true));
        }

        private void OnRevealCardsEvent(RevealCardsEvent e)
        {
            StartCoroutine(CO_SetTileState(ETileState.CLEARED, true));

        }
        private void OnBoardEmptied(EmptyBoardEvent e)
        {
            Destroy(gameObject);
        }

        public void RevealOnClick()
        {
            if (tileState != ETileState.COVERED) return;
            StartCoroutine(CO_SetTileState(ETileState.REVEALED));

        }//Closes RevealOnClick method

        public void OnCoverRevealedTile(CoverRevealedTilesEvent e)
        {
            if (tileState != ETileState.REVEALED) return;
            PlainEventManager.RemoveEventListener<CoverRevealedTilesEvent>(OnCoverRevealedTile);
            PlainEventManager.RemoveEventListener<ClearGroupedTiles>(OnClearGroupedTiles);
            StartCoroutine(CO_SetTileState(ETileState.COVERED, false, .5f));

        }//Closes OnCoverRevealedTile method

        public void OnClearGroupedTiles(ClearGroupedTiles e)
        {
            if (tileState != ETileState.REVEALED) return;
            PlainEventManager.RemoveEventListener<CoverRevealedTilesEvent>(OnCoverRevealedTile);
            PlainEventManager.RemoveEventListener<ClearGroupedTiles>(OnClearGroupedTiles);
            StartCoroutine(CO_SetTileState(ETileState.CLEARED));

        }//Closes OnClearGroupedTiles method

        private void OnStateChanged(ETileState state)
        {
            tileState = state;

            switch (tileState)
            {
                case ETileState.REVEALED:
                    SetCardColor(GetCardColor());
                    PlainEventManager.AddEventListener<CoverRevealedTilesEvent>(OnCoverRevealedTile);
                    PlainEventManager.AddEventListener<ClearGroupedTiles>(OnClearGroupedTiles);
                    PlainEventManager.TriggerEvent(new CardFlippedEvent(this));
                    break;

                case ETileState.CLEARED:
                    PlainEventManager.TriggerEvent(new CardClearedEvent(this));
                    SetCardColor(GetClearedCardColor());
                    break;

            }

        }//Closes OnStateChanged method

        #endregion

        #region Utilities
        private void SetCardColor(Color color)
        {

            if (cardBG) cardBG.color = color;

        }

        public Color GetCardColor()
        {
            return Color.HSVToRGB(data.number / 10f, 1f, .5f);

        }//Closes GetCardColor coroutine
        public Color GetClearedCardColor()
        {
            return Color.HSVToRGB(data.number / 10f, .5f, 1f);

        }//Closes GetCardColor coroutine

        #endregion

        #region  Coroutines

        private IEnumerator CO_SetTileState(ETileState newState, bool randomOffset = false, float delay = 0)
        {
            OnStateChanged(ETileState.TRANSITIONING);

            yield return new WaitForSeconds(delay);
            if (randomOffset) yield return new WaitForSeconds(Random.Range(0f, .5f));
            yield return CO_HandleCardAnimation(newState);

            OnStateChanged(newState);

        }//Closes CO_SetTileState coroutine

        private IEnumerator CO_HandleCardAnimation(ETileState newState)
        {

            switch (newState)
            {
                case ETileState.REVEALED:
                case ETileState.CLEARED:
                    if (animator) animator.SetTrigger("Reveal");
                    break;

                case ETileState.COVERED:
                case ETileState.LOCKED:
                    if (animator) animator.SetTrigger("Cover");
                    break;
            }

            yield return new WaitForSeconds(flipAnimationDuration);

        }//Closes CO_SetTileState coroutine

        #endregion

    }//Closes Tile class
}//Closes Namespace declaration
