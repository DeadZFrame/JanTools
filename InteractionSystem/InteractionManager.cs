using Jan.Core;
using Jan.Events;
using Jan.UI;
using UnityEngine;

namespace Jan.Interaction
{
    public class InteractionManager : JanBehaviour, IInputHandler
    {
        private IInteractable currentInteractable;
        private IInteractable previousInteractable;
        private IInteractionUI _interactionUI;
        private static IInteractionContext _currentContext;

        [SerializeField] private float rayDistance = 10f;

        protected override void OnEnable()
        {
            base.OnEnable();

            EventManager.Register<GameState>(EventNames.OnGameStateChanged, OnGameStateChanged);

            EventManager.Register<int>(EventNames.OnMouseClicked, OnMouseClicked);
            EventManager.Register(EventNames.OnMouseHold, OnMouseHold);
            EventManager.Register<Vector2>(EventNames.OnMouseMoved, OnMouseMoved);
            EventManager.Register<int>(EventNames.OnMouseReleased, OnMouseReleased);
            EventManager.Register<Vector2>(EventNames.OnScroll, OnScroll);
            EventManager.Register(EventNames.OnMouseHover, OnMouseHover);
            EventManager.Register(EventNames.OnMouseHoverOut, OnMouseHoverOut);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            EventManager.UnRegister<GameState>(EventNames.OnGameStateChanged, OnGameStateChanged);

            EventManager.UnRegister<int>(EventNames.OnMouseClicked, OnMouseClicked);
            EventManager.UnRegister(EventNames.OnMouseHold, OnMouseHold);
            EventManager.UnRegister<Vector2>(EventNames.OnMouseMoved, OnMouseMoved);
            EventManager.UnRegister<int>(EventNames.OnMouseReleased, OnMouseReleased);
            EventManager.UnRegister<Vector2>(EventNames.OnScroll, OnScroll);
            EventManager.UnRegister(EventNames.OnMouseHover, OnMouseHover);
            EventManager.UnRegister(EventNames.OnMouseHoverOut, OnMouseHoverOut);
        }

        private void Start()
        {
            UIBusManager.TryGetUIElement(UINames.InteractionUI, out _interactionUI);
        }

        private void Update()
        {
            var gamestate = GameStateManager.CurrentGameState;
            if(gamestate is GameState.UI or GameState.Paused) return;

            var camera = CameraManager.GetCurrentCamera();

            if(camera == null) return;

            var highlightManager = HighlightManager.Instance;
            
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            var isHit = Physics.Raycast(ray, out var hit, rayDistance, LayerMask.GetMask(Layers.Interactable));

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
                    isStateSupported = interactable.SupportedGameState.HasFlag(gamestate);

                    if (isStateSupported)
                    {
                        InteractionLogic(interactable, monoBehaviour);
                    }                    

                    //Debug.Log($"Hit: {hit.collider.gameObject.name}, Interactable: {interactable.GetType().Name}, SupportedGameState: {interactable.SupportedGameState}, CurrentGameState: {gamestate}");
                }
            }
            if(!isHit || !isStateSupported)
            {
                var monoBehaviour = currentInteractable as MonoBehaviour;
                if (monoBehaviour != null)
                {
                    HighlightManager.Instance.Unhighlight(monoBehaviour.transform);
                    EventManager.Trigger(EventNames.OnMouseHoverOut);
                }
                
                currentInteractable = null;

                if(_interactionUI != null)
                {
                    _interactionUI.SetTextAndIcon("", "");
                    _interactionUI.Show(false);
                }
            }
        }

        private void InteractionLogic(IInteractable interactable, MonoBehaviour monoBehaviour)
        {
            //check interactable object change and update interactable
            bool currentInteractableChanged = (currentInteractable != interactable);

            if (currentInteractable != null && currentInteractableChanged)
            {
                previousInteractable = currentInteractable;
                EventManager.Trigger(EventNames.OnMouseHoverOut);
            }

            currentInteractable = interactable;

            if (!interactable.IsActive) return;

            monoBehaviour = interactable as MonoBehaviour;
            if (interactable.HighlightEffect) HighlightManager.Instance.Highlight(monoBehaviour.transform);

            EventManager.Trigger(EventNames.OnMouseHover);

            if (!string.IsNullOrEmpty(interactable.Tooltip))
            {
                if (_interactionUI != null)
                {
                    _interactionUI.SetTextAndIcon(interactable.Tooltip, interactable.HighlightEffect ? InteractionIconNames.LeftClick : "");
                    _interactionUI.Show(true);
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
                if(currentInteractable is IInputHandler inputHandler)
                {
                    inputHandler.OnMouseClicked(buttonIndex);
                }
            }
        }

        public void OnMouseReleased(int buttonIndex)
        {
            if(currentInteractable != null)
            {
                if(currentInteractable is IInputHandler inputHandler)
                {
                    inputHandler.OnMouseReleased(buttonIndex);
                }
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
                if(currentInteractable is IInputHandler inputHandler)
                {
                    inputHandler.OnMouseHold();
                }
            } 
        }

        public void OnMouseMoved(Vector2 mouseWorldPosition)
        {
            if(currentInteractable != null)
            {
                if(currentInteractable is IInputHandler inputHandler)
                {
                    inputHandler.OnMouseMoved(mouseWorldPosition);
                }
            } 
        }

        public void OnScroll(Vector2 scrollValue)
        {
            if(currentInteractable != null)
            {
                if(currentInteractable is IInputHandler inputHandler)
                {
                    inputHandler.OnScroll(scrollValue);
                }
            } 
        }

        public void OnMouseHover()
        {
            if(currentInteractable != null)
            {
                if(currentInteractable is IInputHandler inputHandler)
                {
                    inputHandler.OnMouseHover();
                }
            } 
        }

        public void OnMouseHoverOut()
        {
            if(currentInteractable != null)
            {
                if(currentInteractable is IInputHandler inputHandler)
                {
                    inputHandler.OnMouseHoverOut();
                }
            } 
        }
    }
}
