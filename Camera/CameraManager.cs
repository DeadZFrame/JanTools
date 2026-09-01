using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jan.Core
{
    public class CameraManager : Singleton<CameraManager>
    {
        [SerializeField] private float transitionDuration = 1f;
        private readonly List<CameraHook> cameras = new List<CameraHook>();
        public CameraBase CameraBase { get; private set; }
        public bool Transitioning { get; private set; }

        public void RegisterCamera(CameraHook camera)
        {
            if (!cameras.Contains(camera))
            {
                cameras.Add(camera);
            }
        }

        public void SetMainCamera(CameraBase camera)
        {
            CameraBase = camera;
        }

        public static CameraHook GetCurrentCamera()
        {
            for (int i = Instance.cameras.Count - 1; i >= 0; i--)
            {
                if (Instance.cameras[i].IsActive)
                {
                    return Instance.cameras[i];
                }
            }

            return null;
        }

        public static Camera GetMainCamera()
        {
            return Instance.CameraBase.CameraComponent;
        }

        public static void SwitchCamera<T>() where T : CameraHook
        {
            CameraHook newCamera = null;

            foreach (var camera in Instance.cameras)
            {
                if (camera is T)
                {
                    camera.CameraComponent = GetMainCamera();
                    camera.IsActive = true;

                    newCamera = camera;
                }
                else
                {
                    camera.IsActive = false;
                }
            }

            Instance.Transitioning = true;

            Debug.Log(newCamera.transform);

            Instance.CameraBase.transform.LitMove(newCamera.transform.position, Instance.transitionDuration,  Ease.OutSine);
            Instance.CameraBase.transform.LitRotate(newCamera.transform.rotation, Instance.transitionDuration, Ease.OutSine)
                .OnCompleted(() => { Instance.Transitioning = false; });
        }
    }
}
