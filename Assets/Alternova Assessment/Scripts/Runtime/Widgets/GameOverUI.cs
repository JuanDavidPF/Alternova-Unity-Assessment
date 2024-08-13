
using Alternova.Runtime.Events;
using Payosky.Core.PlainEvent;
using UnityEngine;

namespace Alternova.Runtime.Widegets
{
    [RequireComponent(typeof(CanvasGroup))]
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup cg;

        private void OnValidate()
        {
            if (!cg) TryGetComponent(out cg);
        }//Closes OnValidate method

        private void Start()
        {
            Turn(false);
            PlainEventManager.AddEventListener<GameOverEvent>(OnGameOver);

        }//Closes Start Method


        public void Turn(bool on)
        {
            cg.alpha = on ? 1 : 0;
            cg.interactable = on;
            cg.blocksRaycasts = on;

        }//Closes TurnOff method
        private void OnGameOver(GameOverEvent e)
        {
            Turn(true);

        }//Closes OnGameOver Method

    }//Closes GameOverUI class
}//Closes Namespace declaration
