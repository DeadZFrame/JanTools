using Jan.Core;

namespace Jan.Interaction
{
    public interface IInteractable
    {
        bool IsActive { get; }
        bool HighlightEffect { get; }
        bool IsHoldable { get; }
        string Tooltip { get; }
        GameState SupportedGameState { get; }
        void OnHover();

        void HoverOut();

        void Interact(IInteractionContext interactor, int buttonIndex);
    }
}

