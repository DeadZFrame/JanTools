using Jan.Core;
using Jan.Events;
using UnityEngine;

namespace Jan.Core
{
    public class FPSCamera : Camera
    {
        [SerializeField] private Transform playerBody;
        [SerializeField] private Vector3 offset = new Vector3(0, 1.6f, 0);
        [SerializeField] private float lookSensitivity = 1f;
        [SerializeField] private float minPitch = -80f;
        [SerializeField] private float maxPitch = 80f;

        private float _pitch, _yaw;

        void OnEnable()
        {
            EventManager.Register<Vector2>(EventNames.OnLookInput, OnLookInput);
        }

        void Start()
        {
            CameraManager.Instance.SetCurrentCamera(this);
            GameStateManager.SetGameState(GameState.FPS);
        }

        void OnDisable()
        {
            EventManager.UnRegister<Vector2>(EventNames.OnLookInput, OnLookInput);
        }

        void LateUpdate()
        {
            if(GameStateManager.Instance.CurrentGameState != GameState.FPS) return;
            
            transform.position = playerBody.position + playerBody.TransformDirection(offset);
            transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
        }

        private void OnLookInput(Vector2 lookInput)
        {
            _pitch = Mathf.Clamp(_pitch - lookInput.y * lookSensitivity, minPitch, maxPitch);
            _yaw += lookInput.x * lookSensitivity;
        }
    }

}
