using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jan.Core
{
    public class CameraManager : Singleton<CameraManager>
    {
        private List<CameraBase> cameras = new List<CameraBase>();

        public CameraBase CurrentCamera { get; private set; }

        public void RegisterCamera(CameraBase camera)
        {
            if (!cameras.Contains(camera))
            {
                cameras.Add(camera);
            }
        }

        public void SetCurrentCamera(CameraBase camera)
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

            return currentCamera.CameraComponent;
        }

        public static CameraBase GetCamera()
        {
            return Instance.CurrentCamera;
        }

        public static void SwitchCamera<T>() where T : CameraBase
        {
            CameraBase newCamera = null;

            foreach (var camera in Instance.cameras)
            {
                if (camera is T)
                {
                    newCamera = camera;
                    break;
                }
            }

            if (newCamera == null)
            {
                Debug.LogError($"No camera of type {typeof(T)} found in the scene.");
                return;
            }

            newCamera.CameraComponent.enabled = true;
            newCamera.AudioListener.enabled = true;
            
            var currentCamera = Instance.CurrentCamera;
            if (currentCamera != null)
            {
                currentCamera.CameraComponent.enabled = false;
                currentCamera.AudioListener.enabled = false;
            }

            Instance.SetCurrentCamera(newCamera);
        }
    }
}
