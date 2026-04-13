using Jan.Core;

namespace Jan.InteractionSystem
{
    public interface IInteractable
    {
        bool IsActive { get; }
        bool HighlightEffect { get; }
        bool IsHoldable { get; }
        string Tooltip { get; }
        GameState SupportedGameState { get; }

        void Interact(IInteractionContext interactor, int buttonIndex);
    }
}

