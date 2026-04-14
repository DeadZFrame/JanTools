using UnityEngine;

namespace Jan.Core
{
    public abstract class CameraBase : JanBehaviour
    {
        [field: SerializeField] public Camera CameraComponent { get; private set; }
        [field: SerializeField] public AudioListener AudioListener { get; private set; }

        protected virtual void Awake()
        {
            CameraManager.Instance.RegisterCamera(this);
        }
    }
}