using System.Collections;
using Alternova.Runtime.Events;
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
        private ETileState tileState = ETileState.LOCKED;


        private void OnValidate()
        {

            if (!animator) TryGetComponent(out animator);

        }//Closes Awake method
        private void OnDestroy()
        {
            PlainEventManager.Instance.UnregisterListener<CoverBoardEvent>(OnCoverTile);
            PlainEventManager.Instance.UnregisterListener<RevealCardsEvent>(OnRevealCardsEvent);
            PlainEventManager.Instance.UnregisterListener<ClearBoardEvent>(OnBoardCleared);

        }//Closes Start method

        private void Start()
        {
            PlainEventManager.Instance.RegisterListener<CoverBoardEvent>(OnCoverTile);
            PlainEventManager.Instance.RegisterListener<RevealCardsEvent>(OnRevealCardsEvent);
            PlainEventManager.Instance.RegisterListener<ClearBoardEvent>(OnBoardCleared);

        }//Closes Start method

        public void Initialize(TileData data)
        {
            this.data = data;
            if (tileLabel) tileLabel.text = data.number.ToString();
            if (cardBG) cardBG.color = GetCardColor();

        }//Closes Initialize method


        private void OnCoverTile(CoverBoardEvent e)
        {
            SetTileState(ETileState.COVERED, true);
        }

        private void OnRevealCardsEvent(RevealCardsEvent e)
        {
            SetTileState(ETileState.CLEARED, true);
        }


        public void RevealOnClick()
        {
            if (tileState != ETileState.COVERED) return;
            SetTileState(ETileState.REVEALED);

        }//Closes Reveal method

        private void OnBoardCleared(ClearBoardEvent e)
        {
            Destroy(gameObject);
        }


        public void SetTileState(ETileState newState, bool randomOffset = false)
        {
            StartCoroutine(CO_SetTileState(newState, randomOffset));
        }


        private IEnumerator CO_SetTileState(ETileState newState, bool randomOffset)
        {
            tileState = ETileState.TRANSITIONING;

            if (randomOffset) yield return new WaitForSeconds(Random.Range(0f, 1f));

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

            tileState = newState;
            if (newState == ETileState.REVEALED) PlainEventManager.Instance.TriggerEvent(new CardFlippedEvent(this));


        }//Closes CO_SetTileState coroutine


        public Color GetCardColor()
        {
            return Color.HSVToRGB(data.number / 10f, 1f, .5f);

        }//Closes GetCardColor coroutine

    }//Closes Tile class
}//Closes Namespace declaration
