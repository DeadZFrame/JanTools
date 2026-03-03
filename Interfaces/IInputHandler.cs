using UnityEngine;

namespace Jan.Core
{
    public interface IInputHandler
    {
        void OnMouseClicked();
        void OnMouseReleased();
        void OnMouseHold();
        void OnMouseMoved(Vector2 mousePosition);
        void OnScroll(Vector2 scrollValue);
    }
}