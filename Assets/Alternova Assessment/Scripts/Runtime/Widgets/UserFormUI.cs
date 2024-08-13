using log4net.Layout;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Alternova.Runtime.Widgets
{
    public class UserFormUI : MonoBehaviour
    {
        private string playerInitials = "";
        [SerializeField] private Button playButton;
        [SerializeField] private TMP_InputField userInput;
        [SerializeField] private UnityEvent onSubmit;



        private void Start()
        {
            if (userInput)
            {
                userInput.text = playerInitials;
                userInput.onValueChanged.AddListener(OnUserInputChanged);
            }
            if (playButton)
            {
                playButton.interactable = false;
                playButton.onClick.AddListener(OnPlayButtonClicked);
            }

        }//Clsoes Start method


        private void OnPlayButtonClicked()
        {
            if (playerInitials.Length == 3)
            {
                PlayerState.Instance.SetUsername(playerInitials);
                onSubmit.Invoke();
            }
        }

        private void OnUserInputChanged(string value)
        {
            playerInitials = string.Empty;
            foreach (char c in value)
            {
                if (char.IsLetter(c))
                {
                    playerInitials += char.ToUpper(c);
                }
            }

            if (playerInitials != value)
            {
                userInput.text = playerInitials;
                userInput.caretPosition = playerInitials.Length;
            }

            if (playButton) playButton.interactable = playerInitials.Length == 3;

        }//Closes OnUserInputChanged method


    }//Closes UserForm class
}//Closes Namespace declaration