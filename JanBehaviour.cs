using Jan.Events;
using Jan.InteractionSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Jan.Core
{
    public abstract class JanBehaviour : MonoBehaviour
    {
        public new Transform transform { get; private set; }
        public new GameObject gameObject { get; private set; }

        protected virtual void OnEnable()
        {
            transform = base.transform;
            gameObject = base.gameObject;

            if(this is IInteractable interactable)
            {
                gameObject.SetLayerToChildren(Layers.Interactable);
            }
            
            if(this is IInputHandler inputHandler)
            {
                EventManager.Register<int>(EventNames.OnMouseClicked, inputHandler.OnMouseClicked);
                EventManager.Register(EventNames.OnMouseHold, inputHandler.OnMouseHold);
                EventManager.Register<Vector2>(EventNames.OnMouseMoved, inputHandler.OnMouseMoved);
                EventManager.Register<int>(EventNames.OnMouseReleased, inputHandler.OnMouseReleased);
                EventManager.Register<Vector2>(EventNames.OnScroll, inputHandler.OnScroll);
            }
        }

        protected virtual void OnDisable()
        {
            if(this is IInputHandler inputHandler)
            {
                EventManager.UnRegister<int>(EventNames.OnMouseClicked, inputHandler.OnMouseClicked);
                EventManager.UnRegister(EventNames.OnMouseHold, inputHandler.OnMouseHold);
                EventManager.UnRegister<Vector2>(EventNames.OnMouseMoved, inputHandler.OnMouseMoved);
                EventManager.UnRegister<int>(EventNames.OnMouseReleased, inputHandler.OnMouseReleased);
                EventManager.UnRegister<Vector2>(EventNames.OnScroll, inputHandler.OnScroll);
            }
        }
    }

    public abstract class JanBehaviour<T> : Singleton<T> where T : SerializedMonoBehaviour
    {
        public new Transform transform { get; private set; }
        public new GameObject gameObject { get; private set; }
        
        protected virtual void OnEnable()
        {
            transform = base.transform;
            gameObject = base.gameObject;

            if(this is IInteractable interactable)
            {
                gameObject.SetLayerToChildren(Layers.Interactable);
            }

            if(this is IInputHandler inputHandler)
            {
                EventManager.Register<int>(EventNames.OnMouseClicked, inputHandler.OnMouseClicked);
                EventManager.Register(EventNames.OnMouseHold, inputHandler.OnMouseHold);
                EventManager.Register<Vector2>(EventNames.OnMouseMoved, inputHandler.OnMouseMoved);
                EventManager.Register<int>(EventNames.OnMouseReleased, inputHandler.OnMouseReleased);
                EventManager.Register<Vector2>(EventNames.OnScroll, inputHandler.OnScroll);
            }
        }

        protected virtual void OnDisable()
        {
            if(this is IInputHandler inputHandler)
            {
                EventManager.UnRegister<int>(EventNames.OnMouseClicked, inputHandler.OnMouseClicked);
                EventManager.UnRegister(EventNames.OnMouseHold, inputHandler.OnMouseHold);
                EventManager.UnRegister<Vector2>(EventNames.OnMouseMoved, inputHandler.OnMouseMoved);
                EventManager.UnRegister<int>(EventNames.OnMouseReleased, inputHandler.OnMouseReleased);
                EventManager.UnRegister<Vector2>(EventNames.OnScroll, inputHandler.OnScroll);
            }
        }
    }
}