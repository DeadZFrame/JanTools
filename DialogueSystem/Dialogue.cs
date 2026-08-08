using System;
using Jan.UI;
using UnityEngine;

namespace Jan.Dialogue
{
    [Serializable]
    public class Dialogue<T>
    {
        [field: SerializeField] public T Id { get; private set; }
        [field: SerializeField, TextArea] public string DialogueText { get; private set; }
        private DialogueAction[] _actions;

        public void StartDialogue()
        {
            if(UIBusManager.TryGetUIElement(out IDialogueUI dialogueUI))
            {
                dialogueUI.SetDialogueText(DialogueText);

                if (_actions != null && _actions.Length > 0)
                {
                    foreach (var action in _actions)
                    {
                        dialogueUI.RegisterAction(action.Event.Invoke, action.Text);
                    }
                }

                dialogueUI.Show(true);
            }
        }

        public void RegisterActions(DialogueAction[] actions)
        {
            _actions = actions;
        }
    }

    public class DialogueAction
    {
        public string Text { get; private set; }
        public Action Event { get; private set; }

        public DialogueAction(Action action, string text)
        {
            Event = action;
            Text = text;
        }
    }
}