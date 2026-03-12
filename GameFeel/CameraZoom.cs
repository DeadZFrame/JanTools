using LitMotion;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Giant.Feel
{
    [System.Serializable]
    public class CameraZoom : FeedbackBase
    {
        [field: SerializeField, Tooltip("Reference to the Camera to control")]
        public Camera TargetCamera { get; set; }
        
        [field: SerializeField, Tooltip("Initial zoom value")]
        public float StartZoomValue { get; set; } = 5f;
        
        [field: SerializeField, Tooltip("Target zoom value")]
        public float EndZoomValue { get; set; } = 3f;
        
        [field: SerializeField, Tooltip("True for perspective (FOV), False for orthographic size")]
        public bool IsPerspective { get; set; } = false;
        
        [field: SerializeField, Tooltip("Animation curve for the zoom transition")]
        public AnimationCurve ZoomCurve { get; set; } = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        [field: SerializeField, Tooltip("Whether to return to original zoom after effect")]
        public bool ReturnToOriginal { get; set; } = true;
        
        [field: SerializeField, Tooltip("Duration of return animation"), ShowIf("ReturnToOriginal")]
        public float ReturnDuration { get; set; } = 0.5f;

        private MotionHandle zoomHandle;
        private float originalZoomValue;

        public override FeedbackBase Play(Transform transform)
        {
            if (TargetCamera == null)
            {
                Debug.LogWarning("CameraZoom feedback: No target camera assigned!");
                return this;
            }

            if (zoomHandle.IsActive())
                zoomHandle.Complete();

            originalZoomValue = GetCurrentZoom();

            var cachedEndZoom = EndZoomValue;
            var cachedOriginalZoom = originalZoomValue;
            var cachedReturnDuration = ReturnDuration;
            var cachedReturnToOriginal = ReturnToOriginal;
            var cachedZoomCurve = ZoomCurve;

            SetZoom(StartZoomValue);

            zoomHandle = LMotion.Create(StartZoomValue, EndZoomValue, Duration)
                .WithEase(ZoomCurve)
                .WithDelay(Delay)
                .WithOnComplete(() =>
                {
                    if (cachedReturnToOriginal)
                    {
                        zoomHandle = LMotion.Create(cachedEndZoom, cachedOriginalZoom, cachedReturnDuration)
                            .WithEase(cachedZoomCurve)
                            .Bind(SetZoom);
                    }
                })
                .Bind(SetZoom);

            return this;
        }

        private float GetCurrentZoom()
        {
            return IsPerspective ? TargetCamera.fieldOfView : TargetCamera.orthographicSize;
        }

        private void SetZoom(float value)
        {
            if (IsPerspective)
                TargetCamera.fieldOfView = value;
            else
                TargetCamera.orthographicSize = value;
        }

        public void SetCamera(Camera camera)
        {
            TargetCamera = camera;
        }

        public void SetStartZoom(float zoom)
        {
            StartZoomValue = zoom;
        }

        public void SetEndZoom(float zoom)
        {
            EndZoomValue = zoom;
        }

        public override void Complete()
        {
            if (zoomHandle.IsActive())
                zoomHandle.Complete();
        }

        public override void Stop()
        {
            if (zoomHandle.IsActive())
                zoomHandle.Cancel();
        }
    }
}
