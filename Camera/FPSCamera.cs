using Jan.Core;
using Jan.Events;
using UnityEngine;

namespace Jan.Core
{
    public class FPSCamera : CameraHook
    {
        [SerializeField] private Transform playerBody;
        [SerializeField] private Vector3 offset = new Vector3(0, 1.6f, 0);
        [SerializeField] private float lookSensitivity = 1f;
        [SerializeField] private float minPitch = -80f;
        [SerializeField] private float maxPitch = 80f;
        [SerializeField] private float smoothStrength = 0.1f;

        private float _pitch, _yaw;
        private Vector3 _velocity;

        void OnEnable()
        {
            EventManager.Register<Vector2>(EventNames.OnLookInput, OnLookInput);
        }

        void OnDisable()
        {
            EventManager.UnRegister<Vector2>(EventNames.OnLookInput, OnLookInput);
        }

        protected override void Awake()
        {
            base.Awake();
            
            transform.position = playerBody.position + playerBody.TransformDirection(offset);
            transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);

            CameraManager.SwitchCamera<FPSCamera>();
        }


        void Start()
        {
            GameStateManager.SetGameState(GameState.FPS, true);
        }

        void LateUpdate()
        {            
            if(CameraManager.Instance.Transitioning) return;
            if(GameStateManager.CurrentGameState is GameState.FPS)
            {
                var smoothedPosition = Vector3.SmoothDamp(transform.position, playerBody.position + playerBody.TransformDirection(offset), ref _velocity, smoothStrength);
            
                transform.position = smoothedPosition;
                var targetRotation = Quaternion.Euler(_pitch, _yaw, 0f);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, .5f);
            }            
        }

        private void OnLookInput(Vector2 lookInput)
        {
            if(GameStateManager.CurrentGameState is GameState.FPS)
            {
                _pitch = Mathf.Clamp(_pitch - lookInput.y * lookSensitivity, minPitch, maxPitch);
                _yaw += lookInput.x * lookSensitivity;
            }
        }
    }

}
