using Jan.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Jan.Interaction
{
    public abstract class Interactable : JanBehaviour, IInteractable
    {
        [FoldoutGroup("Interaction Settings")]
        [field: SerializeField, FoldoutGroup("Interaction Settings")] public virtual bool IsActive { get; set; } = true;
        [field: SerializeField, FoldoutGroup("Interaction Settings")] public virtual bool HighlightEffect { get; protected set; } = true;
        [field: SerializeField, FoldoutGroup("Interaction Settings")] public virtual bool IsHoldable { get; protected set; } = false;
        [field: SerializeField, FoldoutGroup("Interaction Settings"), ShowIf(nameof(IsHoldable))] public virtual float HoldTime { get; protected set; } = 1f;
        [field: SerializeField, FoldoutGroup("Interaction Settings")] public virtual string Tooltip { get; protected set; } = "Interact";
        [field: SerializeField, FoldoutGroup("Interaction Settings")] public virtual GameState SupportedGameState { get; protected set; } = GameState.FPS;
        [field: SerializeField, FoldoutGroup("Interaction Settings")] public virtual SubStates SupportedSubStates { get; protected set; } = SubStates.Idle;

        public abstract bool Interact(IInteractionContext interactor, int buttonIndex);
    }
}