using UnityEngine;
    
namespace Jan.Core
{
    public abstract class CameraHook : MonoBehaviour
    {
        public Camera CameraComponent { get; internal set; }
        public bool IsActive { get; internal set; }

        protected virtual void Awake()
        {
            CameraManager.Instance.RegisterCamera(this);
        }
    }
}