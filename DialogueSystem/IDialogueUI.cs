using Jan.UI;
using UnityEngine.Events;

namespace Jan.Dialogue
{
    public interface IDialogueUI : IUIElement
    {
        void RegisterAction(UnityAction action, string text);
        void SetDialogueText(string text);
    }
}