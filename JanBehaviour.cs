using Jan.Events;
using Jan.Interaction;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Jan.Core
{
    public abstract class JanBehaviour : MonoBehaviour
    {
        [SerializeField] protected bool overrideLayer = true;  

        protected virtual void OnEnable()
        {

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
        [SerializeField] private bool overrideLayer = true;

        protected virtual void OnEnable()
        {
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