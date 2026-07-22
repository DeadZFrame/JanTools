using Jan.Events;
using Jan.Interaction;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Jan.Core
{
    public abstract class JanBehaviour : MonoBehaviour
    {
        public new Transform transform { get; private set; }
        public new GameObject gameObject { get; private set; }

        [SerializeField] private bool overrideLayer = true;  

        protected virtual void OnEnable()
        {
            transform = base.transform;
            gameObject = base.gameObject;

            if(this is IInteractable interactable && overrideLayer)
            {
                gameObject.SetLayerToChildren(Layers.Interactable);
            }
        }

        protected virtual void OnDisable()
        {
            
        }
    }

    public abstract class JanBehaviour<T> : Singleton<T> where T : SerializedMonoBehaviour
    {
        public new Transform transform { get; private set; }
        public new GameObject gameObject { get; private set; }
        [SerializeField] private bool overrideLayer = true;

        protected virtual void OnEnable()
        {
            transform = base.transform;
            gameObject = base.gameObject;

            if(this is IInteractable interactable && overrideLayer)
            {
                gameObject.SetLayerToChildren(Layers.Interactable);
            }
        }

        protected virtual void OnDisable()
        {
            
        }
    }
}