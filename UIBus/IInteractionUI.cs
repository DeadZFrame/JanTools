using UnityEngine;

namespace UIBus
{
    public interface IInteractionUI : IUIElement
    {
        void SetTextAndIcon(string text, string iconName);
    }
}