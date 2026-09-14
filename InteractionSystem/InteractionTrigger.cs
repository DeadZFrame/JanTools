using Jan.Core;
using Jan.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Jan.Interaction
{
    public class InteractionTrigger: MonoBehaviour
    {
        [SerializeField] private UnityEvent onMouseClicked;
        [SerializeField] private UnityEvent onMouseReleased;
        [SerializeField] private UnityEvent onMouseHold;
        [SerializeField] private UnityEvent onMouseMoved;
        [SerializeField] private UnityEvent onScroll;
        [SerializeField] private UnityEvent onMouseHover;
        [SerializeField] private UnityEvent onMouseHoverOut;

        void OnEnable()
        {
            EventManager.Register<int>(EventNames.OnMouseClicked, OnMouseClicked);
            EventManager.Register<int>(EventNames.OnMouseReleased, OnMouseReleased);
            EventManager.Register(EventNames.OnMouseHold, OnMouseHold);
            EventManager.Register<Vector2>(EventNames.OnMouseMoved, OnMouseMoved);
            EventManager.Register<Vector2>(EventNames.OnScroll, OnScroll);
            EventManager.Register(EventNames.OnMouseHover, OnMouseHover);
            EventManager.Register(EventNames.OnMouseHoverOut, OnMouseHoverOut);
        }

        void OnDisable()
        {
            EventManager.UnRegister<int>(EventNames.OnMouseClicked, OnMouseClicked);
            EventManager.UnRegister<int>(EventNames.OnMouseReleased, OnMouseReleased);
            EventManager.UnRegister(EventNames.OnMouseHold, OnMouseHold);
            EventManager.UnRegister<Vector2>(EventNames.OnMouseMoved, OnMouseMoved);
            EventManager.UnRegister<Vector2>(EventNames.OnScroll, OnScroll);
            EventManager.UnRegister(EventNames.OnMouseHover, OnMouseHover);
            EventManager.UnRegister(EventNames.OnMouseHoverOut, OnMouseHoverOut);
        }

        private void OnMouseClicked(int buttonIndex)
        {
            onMouseClicked?.Invoke();
        }

        private void OnMouseReleased(int buttonIndex)
        {
            onMouseReleased?.Invoke();
        }

        private void OnMouseHold()
        {
            onMouseHold?.Invoke();
        }

        private void OnMouseMoved(Vector2 mousePosition)
        {
            onMouseMoved?.Invoke();
        }

        private void OnScroll(Vector2 scrollValue)
        {
            onScroll?.Invoke();
        }

        private void OnMouseHover()
        {
            onMouseHover?.Invoke();
        }

        private void OnMouseHoverOut()
        {
            onMouseHoverOut?.Invoke();
        }
    }
}