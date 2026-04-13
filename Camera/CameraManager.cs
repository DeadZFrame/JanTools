using UnityEngine;

namespace Jan.Core
{
    public class CameraManager : Singleton<CameraManager>
    {
        public ICamera CurrentCamera { get; private set; }

        public void SetCurrentCamera(ICamera camera)
        {
            CurrentCamera = camera;
        }

        public static Camera GetCurrentCamera()
        {
            var currentCamera = Instance.CurrentCamera;

            if(currentCamera == null)
            {
                Debug.LogWarning("Current camera is not set.");
                return null;
            }

            return currentCamera.Camera;
        }

        public static ICamera GetICamera()
        {
            return Instance.CurrentCamera;
        }

        public static void SwitchCamera(ICamera newCamera)
        {
            newCamera.Camera.enabled = true;
            newCamera.AudioListener.enabled = true;
            
            var currentCamera = Instance.CurrentCamera;
            if (currentCamera != null)
            {
                currentCamera.Camera.enabled = false;
                currentCamera.AudioListener.enabled = false;
            }

            Instance.SetCurrentCamera(newCamera);
        }
    }
}
