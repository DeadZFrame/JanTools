using System;
using Jan.Core;
using Jan.Localization;
using Jan.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Jan.Dialogue
{
    [Serializable]
    public class Dialogue<T>
    {
        [field: SerializeField] public T Id { get; private set; }
        [SerializeField, ValueDropdown(nameof(GetLocalizationContexts))] private string localizationContext;
        [SerializeField, ValueDropdown(nameof(GetDialogueIds))] private string dialogueId;

        private string[] GetLocalizationContexts => GlobalsUtils.GetNames(typeof(LocalizationKeys));
        private string[] GetDialogueIds => LocalizationManager.GetContext(localizationContext);
        [SerializeField] private DialogueAction[] _actions;

        public void StartDialogue()
        {
            if(UIBusManager.TryGetUIElement(out IDialogueUI dialogueUI))
            {
                dialogueUI.Show(true);

                dialogueUI.SetDialogueText(LocalizationManager.GetLocalizedValue(localizationContext, dialogueId));

                if (_actions != null && _actions.Length > 0)
                {
                    foreach (var action in _actions)
                    {
                        dialogueUI.RegisterAction(action.Event.Invoke, LocalizationManager.GetLocalizedValue(LocalizationKeys.DialogueActions, action.TextId));
                    }
                }
            }
        }

        public void RegisterActions(DialogueAction[] actions)
        {
            _actions = actions;
        }
    }

    [Serializable]
    public class DialogueAction
    {
        [field: SerializeField, ValueDropdown(nameof(GetDialogueIds))] public string TextId { get; private set; }
        private string[] GetDialogueIds => LocalizationManager.GetContext(LocalizationKeys.DialogueActions);
        [field: SerializeField] public UnityEvent Event { get; private set; } = new UnityEvent();

        public DialogueAction(UnityAction action, string text)
        {
            Event.AddListener(action);
            TextId = text;
        }
    }
}