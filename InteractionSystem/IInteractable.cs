using Jan.Core;
using System.Collections.Generic;

namespace Jan.Interaction
{
    public interface IInteractable
    {
        bool IsActive { get; set; }
        bool HighlightEffect { get; }
        bool IsHoldable { get; }
        string Tooltip { get; }
        GameState SupportedGameState { get; }
        List<SubStates> SupportedSubStates { get; }

        bool Interact(IInteractionContext interactor, int buttonIndex);
    }
}

