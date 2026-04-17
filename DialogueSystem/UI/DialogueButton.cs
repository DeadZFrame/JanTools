using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Jan.Dialogue
{
    public class DialogueButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI text;

        public void Initialize(string buttonText, UnityAction onClickAction)
        {
            text.SetText(buttonText);
            button.onClick.AddListener(onClickAction);
        }

        public void ClearListeners()
        {
            button.onClick.RemoveAllListeners();
        }
    }
}