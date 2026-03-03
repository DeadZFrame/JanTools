// using Sirenix.OdinInspector;
// using UnityEngine;
// using DG.Tweening;

// namespace Giant.Feel
// {
//     [System.Serializable]
//     public class CameraZoom : FeedbackBase
//     {
//         [field: SerializeField, Tooltip("Reference to the Cinemachine Virtual Camera to control")]
//         public CinemachineVirtualCamera TargetCamera { get; set; }
        
//         [field: SerializeField, Tooltip("Initial zoom value")]
//         public float StartZoomValue { get; set; } = 5f;
        
//         [field: SerializeField, Tooltip("Target zoom value")]
//         public float EndZoomValue { get; set; } = 3f;
        
//         [field: SerializeField, Tooltip("True for perspective (FOV), False for orthographic size")]
//         public bool IsPerspective { get; set; } = false;
        
//         [field: SerializeField, Tooltip("Animation curve for the zoom transition")]
//         public AnimationCurve ZoomCurve { get; set; } = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
//         [field: SerializeField, Tooltip("Whether to return to original zoom after effect")]
//         public bool ReturnToOriginal { get; set; } = true;
        
//         [field: SerializeField, Tooltip("Duration of return animation"), ShowIf("ReturnToOriginal")]
//         public float ReturnDuration { get; set; } = 0.5f;

//         private Tween zoomTween;
//         private float originalZoomValue;

//         public override FeedbackBase Play(Transform transform)
//         {
//             if (TargetCamera == null)
//             {
//                 Debug.LogWarning("CameraZoom feedback: No target camera assigned!");
//                 return this;
//             }

//             // Kill any existing tweens
//             zoomTween?.Complete();

//             // Store the original zoom value
//             originalZoomValue = GetCurrentZoom();

//             // Create a tween sequence
//             Sequence zoomSequence = DOTween.Sequence();

//             // Add initial delay if needed
//             if (Delay > 0)
//                 zoomSequence.AppendInterval(Delay);

//             // Add the zoom tween
//             zoomSequence.Append(DOVirtual.Float(StartZoomValue, EndZoomValue, Duration, SetZoom).SetEase(ZoomCurve));

//             // Add return tween if enabled
//             if (ReturnToOriginal)
//             {
//                 zoomSequence.Append(DOVirtual.Float(EndZoomValue, originalZoomValue, ReturnDuration, SetZoom).SetEase(ZoomCurve));
//             }

//             // Store the tween reference
//             zoomTween = zoomSequence;

//             return this;
//         }

//         private float GetCurrentZoom()
//         {
//             return IsPerspective ? TargetCamera.m_Lens.FieldOfView : TargetCamera.m_Lens.OrthographicSize;
//         }

//         private void SetZoom(float value)
//         {
//             if (IsPerspective)
//                 TargetCamera.m_Lens.FieldOfView = value;
//             else
//                 TargetCamera.m_Lens.OrthographicSize = value;
//         }

//         public void SetCamera(CinemachineVirtualCamera camera)
//         {
//             TargetCamera = camera;
//         }

//         public override void Complete()
//         {
//             // Complete and kill any active zoom tween
//             if (zoomTween != null && zoomTween.IsActive())
//             {
//                 zoomTween.Complete();
//                 zoomTween = null;
//             }
//         }
//     }
// }
