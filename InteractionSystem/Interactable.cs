using Jan.Core;
using Jan.Events;
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

        protected override void OnEnable()
        {
            base.OnEnable();
            EventManager.Register<GameState>(EventNames.OnGameStateChanged, OnGameStateChanged);
            EventManager.Register<SubStates>(EventNames.OnGameStateChanged, OnSubStateChanged);

            if(overrideLayer) gameObject.SetLayer(SupportedGameState.HasFlag(SupportedGameState) ? Layers.Interactable : Layers.Default);
            if(overrideLayer) gameObject.SetLayer(SupportedSubStates.HasFlag(SupportedSubStates) ? Layers.Interactable : Layers.Default);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            EventManager.UnRegister<GameState>(EventNames.OnGameStateChanged, OnGameStateChanged);
            EventManager.UnRegister<SubStates>(EventNames.OnGameStateChanged, OnSubStateChanged);
        }

        protected virtual void OnGameStateChanged(GameState newState)
        {
            if(overrideLayer) gameObject.SetLayer(SupportedGameState.HasFlag(newState) ? Layers.Interactable : Layers.Default);
            if(overrideLayer) gameObject.SetLayer(SupportedSubStates.HasFlag(GameStateManager.CurrentSubState) ? Layers.Interactable : Layers.Default);
        }
        
        protected virtual void OnSubStateChanged(SubStates newSubState)
        {
            if(overrideLayer) gameObject.SetLayer(SupportedGameState.HasFlag(GameStateManager.CurrentGameState) ? Layers.Interactable : Layers.Default);
            if(overrideLayer) gameObject.SetLayer(SupportedSubStates.HasFlag(newSubState) ? Layers.Interactable : Layers.Default);
        }
    }
}