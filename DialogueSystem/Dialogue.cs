using System;
using Jan.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Jan.Dialogue
{
    [Serializable]
    public class Dialogue<T>
    {
        [Serializable]
        public class DialogueAction
        {
            [field: SerializeField] public string Text { get; private set; }
            [field: SerializeField] public UnityEvent Event { get; private set; }

            public DialogueAction(UnityEvent action, string text)
            {
                Event = action;
                Text = text;
            }
        }

        [field: SerializeField] public T Id { get; private set; }
        [field: SerializeField] public string DialogueText { get; private set; }
        [field: SerializeField] public DialogueAction[] Actions { get; private set; }

        public void StartDialogue()
        {
            if(UIBusManager.TryGetUIElement(out IDialogueUI dialogueUI))
            {
                dialogueUI.SetDialogueText(DialogueText);

                if (Actions != null && Actions.Length > 0)
                {
                    foreach (var action in Actions)
                    {
                        dialogueUI.RegisterAction(action.Event.Invoke, action.Text);
                    }
                }

                dialogueUI.Show(true);
            }
        }
    }
}