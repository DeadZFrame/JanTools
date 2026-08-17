using Jan.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Jan.Interaction
{
    public abstract class Interactable : JanBehaviour, IInteractable
    {
        [FoldoutGroup("Interaction Settings")]
        [field: SerializeField, FoldoutGroup("Interaction Settings")] public virtual bool IsActive { get; set; } = true;
        [field: SerializeField, FoldoutGroup("Interaction Settings")] public virtual bool HighlightEffect { get; private set; } = true;
        [field: SerializeField, FoldoutGroup("Interaction Settings")] public virtual bool IsHoldable { get; private set; } = false;
        [field: SerializeField, FoldoutGroup("Interaction Settings"), ShowIf(nameof(IsHoldable))] public virtual float HoldTime { get; private set; } = 1f;
        [field: SerializeField, FoldoutGroup("Interaction Settings")] public virtual string Tooltip { get; private set; } = "Interact";
        [field: SerializeField, FoldoutGroup("Interaction Settings")] public virtual GameState SupportedGameState { get; private set; } = GameState.FPS;
        [field: SerializeField, FoldoutGroup("Interaction Settings")] public virtual SubStates SupportedSubStates { get; private set; } = SubStates.Idle;

        public abstract bool Interact(IInteractionContext interactor, int buttonIndex);
    }
}