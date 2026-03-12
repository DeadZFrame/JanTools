using UnityEngine;

namespace Jan.Core
{
    public interface IInputHandler
    {
        void OnMouseClicked(int buttonIndex);
        void OnMouseReleased(int buttonIndex);
        void OnMouseHold();
        void OnMouseMoved(Vector2 mousePosition);
        void OnScroll(Vector2 scrollValue);
    }
}