using Jan.Core;
using Jan.Events;
using UnityEngine;

namespace Jan.InteractionSystem
{
    public class InteractionManager : JanBehaviour, IInputHandler
    {
        private IInteractable currentInteractable;

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

                    if(!string.IsNullOrEmpty(interactable.Tooltip))
                    {
                        EventManager.Trigger(EventNames.OnDetectInteractable, (interactable.Tooltip, interactable.HighlightEffect ? InteractionIconNames.LeftClick : ""));
                    }
                }
            }
            else
            {
                var monoBehaviour = currentInteractable as MonoBehaviour;
                if (monoBehaviour != null)
                {
                    HighlightManager.Instance.Unhighlight(monoBehaviour.transform);
                }
                
                currentInteractable = null;

                EventManager.Trigger(EventNames.OnDetectInteractable, ("", ""));
            }
        }

        public void OnMouseClicked()
        {
            if(currentInteractable != null && !currentInteractable.IsHoldable)
            {
                currentInteractable.Interact();
            }

            if(currentInteractable != null)
            {
                currentInteractable.Trigger(EventNames.OnMouseClicked);
            }
        }

        public void OnMouseRightClicked()
        {
            if(currentInteractable != null)
            {
                currentInteractable.RightClickInteract();
            }

            if(currentInteractable != null)
            {
                currentInteractable.Trigger(EventNames.OnMouseRightClicked);
            }
        }

        public void OnMouseReleased()
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
                currentInteractable.Interact();
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
