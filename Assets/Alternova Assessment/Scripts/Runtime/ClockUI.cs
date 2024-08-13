using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Alternova.Runtime
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ClockUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI clockLabel;
        private void OnValidate()
        {
            if (!clockLabel) TryGetComponent(out clockLabel);
        }//Closes OnValidate method

        private void Update()
        {
            clockLabel.text = GameState.Instance.GetFormattedTime();
        }

    }//Closes ClockUI class
}//Closes Namespace declaration
