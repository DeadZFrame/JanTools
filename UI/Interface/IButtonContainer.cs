using UnityEngine.Events;

namespace Jan.UI
{
    public interface IButtonContainer : IUIElement
    {
        void RegisterButton(UnityAction action);
        void RemoveAllListeners();
    }
}