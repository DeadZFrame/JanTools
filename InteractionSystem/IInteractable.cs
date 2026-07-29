using Jan.Core;

namespace Jan.Interaction
{
    public interface IInteractable
    {
        bool IsActive { get; set; }
        bool HighlightEffect { get; }
        bool IsHoldable { get; }
        string Tooltip { get; }
        GameState SupportedGameState { get; }

        void Interact(IInteractionContext interactor, int buttonIndex);
    }
}

