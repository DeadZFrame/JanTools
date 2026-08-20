using Jan.UI;

namespace Jan.Interaction
{
    public interface IInteractionUI : IUIElement
    {
        void SetTextAndIcon(string text, string iconName);
        void SetInteractionProgress(float progress);
    }
}