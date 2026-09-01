using UnityEngine;

namespace Jan.Core
{
    [DefaultExecutionOrder(-5)]
    public class CameraBase : JanBehaviour
    {
        [field: SerializeField] public Camera CameraComponent { get; private set; }
        [field: SerializeField] public AudioListener AudioListener { get; private set; }

        void Awake()
        {
            CameraManager.Instance.SetMainCamera(this);
        }

        void LateUpdate()
        {
            if(CameraManager.Instance.Transitioning) return;
            var currentHook = CameraManager.GetCurrentCamera();
            if(currentHook == null) return;
            
            transform.SetPositionAndRotation(currentHook.transform.position, currentHook.transform.rotation);
        }
    }
}