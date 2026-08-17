using Jan.Core;
using Jan.Events;
using Jan.Feel;
using Jan.Tasks;
using Jan.UI;
using NUnit.Framework;
using UI;
using UnityEngine;

namespace Jan.Interaction
{
    public class InteractionManager : JanBehaviour<InteractionManager>, IInputHandler
    {
        [SerializeField] private LayerMask inputHandlerDetectionLayerMask = 1 << 0;
        private IInteractable currentInteractable;
        public static IInteractable CurrentInteractable => Instance.currentInteractable;
        private IInputHandler currentInputHandler;
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
            GameStateManager.SetSubState(SubStates.Idle);
        }

        private void Update()
        {
            var gamestate = GameStateManager.CurrentGameState;
            var substate = GameStateManager.CurrentSubState;
            if(gamestate is GameState.UI or GameState.Paused) return;

            var camera = CameraManager.GetCurrentCamera();

            if(camera == null) return;

            var highlightManager = HighlightManager.Instance;
            
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            var isHit = Physics.Raycast(ray, out var hit, rayDistance, LayerMask.GetMask(Layers.Interactable));

            bool isStateSupported = false;
            bool isSubStateSupported = false;
            bool isActive = false;

            if (isHit)
            {
                if (hit.collider.gameObject.TryGetComponentInParentChildren(out IInteractable interactable))
                {
                    MonoBehaviour monoBehaviour = currentInteractable as MonoBehaviour;

                    if(monoBehaviour != null && currentInteractable != interactable)
                    {
                        highlightManager.Unhighlight(monoBehaviour.transform);
                    }

                    isStateSupported = interactable.SupportedGameState.HasFlag(gamestate);
                    isSubStateSupported = interactable.SupportedSubStates.HasFlag(substate);

                    isActive = interactable.IsActive;

                    if (isStateSupported && isSubStateSupported && isActive)
                    {
                        InteractionLogic(interactable);
                    }    

                    //Debug.Log($"Hit: {hit.collider.gameObject.name}, Interactable: {interactable.GetType().Name}, SupportedGameState: {interactable.SupportedGameState}, CurrentGameState: {gamestate}");
                }
            }

            if(!isHit || !isStateSupported || !isSubStateSupported || !isActive)
            {
                var monoBehaviour = currentInteractable as MonoBehaviour;
                if (monoBehaviour != null)
                {
                    HighlightManager.Instance.Unhighlight(monoBehaviour.transform);
                }
                
                currentInteractable = null;

                if(_interactionUI != null)
                {
                    _interactionUI.SetTextAndIcon("", "");
                    _interactionUI.Show(false);
                }
            }

            GetInputHandler(ray);
        }

        private void InteractionLogic(IInteractable interactable)
        {
            if(currentInteractable == interactable) return;
            currentInteractable = interactable;

            var monoBehaviour = interactable as MonoBehaviour;
            if (interactable.HighlightEffect) HighlightManager.Instance.Highlight(monoBehaviour.transform);

            if (!string.IsNullOrEmpty(interactable.Tooltip))
            {
                if (_interactionUI != null)
                {
                    _interactionUI.SetTextAndIcon(interactable.Tooltip, interactable.HighlightEffect ? InteractionIconNames.LeftClick : "");
                    _interactionUI.Show(true);
                }
            }
        }

        private void GetInputHandler(Ray ray)
        {            
            var isHit = Physics.Raycast(ray, out var hit, rayDistance, inputHandlerDetectionLayerMask);

            if (isHit)
            {
                if (hit.collider.gameObject.TryGetComponentInParentChildren(out IInputHandler inputHandler))
                {                 
                    if(currentInputHandler != inputHandler)
                    {
                        currentInputHandler?.OnMouseHoverOut();
                        currentInputHandler = inputHandler;
                        currentInputHandler?.OnMouseHover();
                    }
                }
                else
                {
                    if(currentInputHandler != null)
                    {
                        currentInputHandler?.OnMouseHoverOut();
                        currentInputHandler = null;
                    }
                }
            }
            if(!isHit)
            {       
                if(currentInputHandler != null)
                {
                    currentInputHandler?.OnMouseHoverOut();
                    currentInputHandler = null;
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
        
        public static void SetWarning(string warningText, bool isWarning)
        {
            if (UIBusManager.TryGetUIElement(out IWarningUI warningUI))
            {
                warningUI.Show(true);
                warningUI.SetWarningText(warningText, isWarning);
            }
        }

        public void OnMouseClicked(int buttonIndex)
        {
            if(currentInteractable != null && !currentInteractable.IsHoldable)
            {
                var response = currentInteractable.Interact(_currentContext, buttonIndex);
                if (!response) 
                {
                    var invalidInteractable = (currentInteractable as MonoBehaviour).transform;
                    HighlightManager.Instance.HighlightInvalid(invalidInteractable);
                    FeedbackManager.Instance.PlayFeedback(EventNames.OnInvalidInteraction, invalidInteractable);
                }
            }

            currentInputHandler?.OnMouseClicked(buttonIndex);
        }

        public void OnMouseReleased(int buttonIndex)
        {
            _heldTime = 0;
            _interactionUI.SetInteractionProgress(0);
            currentInputHandler?.OnMouseReleased(buttonIndex);
        }

        private float _heldTime;

        public void OnMouseHold()
        {
            if(currentInteractable != null && currentInteractable.IsHoldable)
            {
                _heldTime += Time.deltaTime;
                _interactionUI.SetInteractionProgress(_heldTime / currentInteractable.HoldTime);
                if(_heldTime >= currentInteractable.HoldTime)
                {
                    currentInteractable.Interact(_currentContext, 0);
                    _heldTime = 0;
                    currentInteractable.IsActive = false;
                    var interactable = currentInteractable;
                    Timed.CallDelayed(2f, () =>  interactable.IsActive = true, (interactable as MonoBehaviour).gameObject);
                }
            }
            
            currentInputHandler?.OnMouseHold(); 
        }

        public void OnMouseMoved(Vector2 mouseWorldPosition)
        {
            currentInputHandler?.OnMouseMoved(mouseWorldPosition); 
        }

        public void OnScroll(Vector2 scrollValue)
        {
            currentInputHandler?.OnScroll(scrollValue); 
        }

        public void OnMouseHover()
        {
            currentInputHandler?.OnMouseHover();
        }

        public void OnMouseHoverOut()
        {
            currentInputHandler?.OnMouseHoverOut();
        }
    }
}
