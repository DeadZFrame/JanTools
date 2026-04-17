using Jan.Core;
using Jan.Events;
using Jan.UI;
using UnityEngine;

namespace Jan.Interaction
{
    public class InteractionManager : JanBehaviour, IInputHandler
    {
        private IInteractable currentInteractable;
        private IInteractionUI _interactionUI;
        private static IInteractionContext _currentContext;

        [SerializeField] private float rayDistance = 10f;

        protected override void OnEnable()
        {
            base.OnEnable();

            EventManager.Register<GameState>(EventNames.OnGameStateChanged, OnGameStateChanged);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            EventManager.UnRegister<GameState>(EventNames.OnGameStateChanged, OnGameStateChanged);
        }

        private void Start()
        {
            UIBusManager.TryGetUIElement(UINames.InteractionUI, out _interactionUI);
        }

        private void Update()
        {
            var gamestate = GameStateManager.Instance.CurrentGameState;
            if(gamestate is GameState.UI or GameState.Paused) return;

            var camera = CameraManager.GetCurrentCamera();

            var highlightManager = HighlightManager.Instance;
            
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            var isHit = Physics.Raycast(ray, out RaycastHit hit, rayDistance, LayerMask.GetMask(Layers.Interactable));

            bool isStateSupported = false;

            if (isHit)
            {
                var monoBehaviour = currentInteractable as MonoBehaviour;
                if (monoBehaviour != null)
                {
                    highlightManager.Unhighlight(monoBehaviour.transform);
                }

                if (hit.collider.gameObject.TryGetComponentInParentChildren(out IInteractable interactable))
                {
                    isStateSupported = interactable.SupportedGameState == gamestate || interactable.SupportedGameState == GameState.Any;

                    if(isStateSupported)
                    {
                        currentInteractable = interactable;

                        monoBehaviour = interactable as MonoBehaviour;
                        if(interactable.HighlightEffect) highlightManager.Highlight(monoBehaviour.transform);

                        interactable.OnHover();

                        if(!string.IsNullOrEmpty(interactable.Tooltip))
                        {
                            if(_interactionUI != null)
                            {
                                _interactionUI.SetTextAndIcon(interactable.Tooltip, interactable.HighlightEffect ? InteractionIconNames.LeftClick : "");
                                _interactionUI.Show(true);
                            }
                        }
                    }                    
                }
            }
            
            if(!isHit || !isStateSupported)
            {
                var monoBehaviour = currentInteractable as MonoBehaviour;
                if (monoBehaviour != null)
                {
                    HighlightManager.Instance.Unhighlight(monoBehaviour.transform);
                    currentInteractable.HoverOut();
                }
                
                currentInteractable = null;

                if(_interactionUI != null)
                {
                    _interactionUI.SetTextAndIcon("", "");
                    _interactionUI.Show(false);
                }
            }
        }

        private void OnGameStateChanged(GameState newState)
        {
            if(currentInteractable != null)
            {
                var monoBehaviour = currentInteractable as MonoBehaviour;
                if (monoBehaviour != null)
                {
                    HighlightManager.Instance.Unhighlight(monoBehaviour.transform);
                }

                currentInteractable = null;
            }
            
            if(_interactionUI != null)
            {
                _interactionUI.SetTextAndIcon("", "");
                _interactionUI.Show(false);
            }
        }

        public static void SetContext(IInteractionContext interactor)
        {
            _currentContext = interactor;
        }

        public void OnMouseClicked(int buttonIndex)
        {
            if(currentInteractable != null && !currentInteractable.IsHoldable)
            {
                currentInteractable.Interact(_currentContext, buttonIndex);
            }

            if(currentInteractable != null)
            {
                currentInteractable.Trigger(EventNames.OnMouseClicked);
            }
        }

        public void OnMouseReleased(int buttonIndex)
        {
            if(currentInteractable != null)
            {
                currentInteractable.Trigger(EventNames.OnMouseReleased);
            }
        }

        public void OnMouseHold()
        {
            if(currentInteractable != null && currentInteractable.IsHoldable)
            {
                currentInteractable.Interact(_currentContext, 0);
            }
            
            if(currentInteractable != null)
            {
                currentInteractable.Trigger(EventNames.OnMouseHold);
            } 
        }

        public void OnMouseMoved(Vector2 mouseWorldPosition)
        {
            
        }

        public void OnScroll(Vector2 scrollValue)
        {
            
        }
    }
}
