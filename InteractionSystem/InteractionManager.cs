using Jan.Core;
using Jan.Events;
using UIBus;
using UnityEngine;

namespace Jan.InteractionSystem
{
    public class InteractionManager : JanBehaviour, IInputHandler
    {
        public Texture2D defaultCursor;

        private IInteractable currentInteractable;
        private IInteractionUI _interactionUI;

        private void Start()
        {
            UIBusManager.Instance.TryGetUIElement(UINames.InteractionUI, out _interactionUI);
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask(Layers.Interactable)))
            {
                var monoBehaviour = currentInteractable as MonoBehaviour;
                if (monoBehaviour != null)
                {
                    HighlightManager.Instance.Unhighlight(monoBehaviour.transform);
                }

                if (hit.collider.gameObject.TryGetComponentInParentChildren(out IInteractable interactable))
                {
                    if(interactable == null) return;
                    currentInteractable = interactable;

                    monoBehaviour = interactable as MonoBehaviour;
                    if(interactable.HighlightEffect) HighlightManager.Instance.Highlight(monoBehaviour.transform);

                    interactable.OnHover();

                    if(!string.IsNullOrEmpty(interactable.Tooltip))
                    {
                        if(_interactionUI != null)
                        {
                            _interactionUI.SetTextAndIcon(interactable.Tooltip, interactable.HighlightEffect ? InteractionIconNames.LeftClick : "");
                        }
                    }
                }
            }
            else
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
                }
            }
        }

        public void OnMouseClicked(int buttonIndex)
        {
            if(currentInteractable != null && !currentInteractable.IsHoldable)
            {
                currentInteractable.Interact(buttonIndex);
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
                currentInteractable.Interact(0);
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
