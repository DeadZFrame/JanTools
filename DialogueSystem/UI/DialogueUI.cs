using System.Collections.Generic;
using Jan.Pool;
using Jan.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Jan.Dialogue
{
    public class DialogueUI : UIElement, IDialogueUI
    {
        [SerializeField] private DialogueButton buttonPrefab;
        [SerializeField] private VerticalLayoutGroup layoutGroup;
        [SerializeField] private TextMeshProUGUI dialogueText;

        private readonly List<DialogueButton> _buttons = new List<DialogueButton>();

        public void SetDialogueText(string text)
        {
            dialogueText.SetText(text);
        }

        public void RegisterAction(UnityAction action, string text)
        {
            var button = JanPool.Spawn(buttonPrefab, layoutGroup.transform);
            button.Initialize(text, action);

            _buttons.Add(button);
        }

        public override void Show(bool show)
        {
            base.Show(show);

            if(show) return;

            foreach (var button in _buttons)        
            {
                button.ClearListeners();
                JanPool.Despawn(button);
            }

            _buttons.Clear();
        }
    }
}
