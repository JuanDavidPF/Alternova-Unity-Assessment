
using TMPro;
using UnityEngine;

namespace Alternova.Runtime.Widegets
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreLabel;
        private void OnValidate()
        {
            if (!scoreLabel) TryGetComponent(out scoreLabel);
        }//Closes OnValidate method

        private void Update()
        {
            scoreLabel.text = $"Score: {PlayerState.Instance.Score}";
        }

    }//Closes ScoreUI class
}//Closes Namespace declaration
