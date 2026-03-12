using System;
using System.Collections.Generic;
using Jan.Tasks;
using LitMotion;
using LitMotion.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Giant.Feel
{
    [Serializable]
    public class ObjectMove : FeedbackBase
    {
        // Static dictionary to track active motion handles by transform
        private static Dictionary<Transform, MotionHandle> activeMotionHandles = new Dictionary<Transform, MotionHandle>();

        [SerializeField] private bool move, rotate;
        [SerializeField, ShowIf(nameof(move))] private Vector3 moveOffset = Vector3.zero;
        [SerializeField, ShowIf(nameof(rotate))] private Vector3 rotationOffset = Vector3.zero;
        [SerializeField] private bool useLocalSpace = false;
        
        // Starting position support
        [SerializeField] private bool useStartingPosition = false;
        [SerializeField, ShowIf(nameof(useStartingPosition))] private Vector3 customStartingPosition = Vector3.zero;
        [SerializeField] private bool useStartingRotation = false;
        [SerializeField, ShowIf(nameof(useStartingRotation))] private Vector3 customStartingRotation = Vector3.zero;
        
        [SerializeField] private Ease ease = Ease.Linear;
        [SerializeField] private bool returnToStart = true;
        [SerializeField] private LoopType loopType = LoopType.Restart;
        [SerializeField] private int loopCount = 1;

        public override FeedbackBase Play(Transform transform)
        {
            // Cancel existing motion if there's one already playing on this transform
            if (activeMotionHandles.TryGetValue(transform, out MotionHandle existingHandle) && existingHandle.IsActive())
            {
                existingHandle.Complete();
                activeMotionHandles.Remove(transform);
            }

            // Check if the transform is a RectTransform
            RectTransform rectTransform = transform as RectTransform;
            bool isUI = rectTransform != null;
            Vector2 _startAnchoredPosition = Vector2.zero;
            Vector3 _startAnchoredPosition3D = Vector3.zero;

            if (isUI)
            {
                _startAnchoredPosition = rectTransform.anchoredPosition;
                _startAnchoredPosition3D = rectTransform.anchoredPosition3D;
            }

            var _startPosition = transform.position;
            var _startLocalPosition = transform.localPosition;
            var _startRotation = transform.rotation;
            var _startLocalRotation = transform.localRotation;

            // Apply custom starting positions if specified
            if (useStartingPosition)
            {
                if (useLocalSpace)
                {
                    if (isUI)
                    {
                        _startAnchoredPosition = (Vector2)customStartingPosition;
                        _startAnchoredPosition3D = customStartingPosition;
                        rectTransform.anchoredPosition = _startAnchoredPosition;
                    }
                    else
                    {
                        _startLocalPosition = customStartingPosition;
                        transform.localPosition = _startLocalPosition;
                    }
                }
                else
                {
                    if (isUI)
                    {
                        _startAnchoredPosition3D = customStartingPosition;
                        rectTransform.anchoredPosition3D = _startAnchoredPosition3D;
                        _startAnchoredPosition = (Vector2)_startAnchoredPosition3D;
                    }
                    else
                    {
                        _startPosition = customStartingPosition;
                        transform.position = _startPosition;
                    }
                }
            }

            if (useStartingRotation)
            {
                if (useLocalSpace)
                {
                    _startLocalRotation = Quaternion.Euler(customStartingRotation);
                    transform.localRotation = _startLocalRotation;
                }
                else
                {
                    _startRotation = Quaternion.Euler(customStartingRotation);
                    transform.rotation = _startRotation;
                }
            }

            // Cache target positions and rotations
            Vector2 _targetAnchoredPosition = _startAnchoredPosition + (Vector2)moveOffset;
            Vector3 _targetAnchoredPosition3D = _startAnchoredPosition3D + moveOffset;
            Vector3 _targetLocalPosition = _startLocalPosition + moveOffset;
            Vector3 _targetPosition = _startPosition + moveOffset;
            Vector3 _targetLocalRotation = _startLocalRotation.eulerAngles + rotationOffset;
            Vector3 _targetRotation = _startRotation.eulerAngles + rotationOffset;

            // Create motion handles for movement and rotation
            MotionHandle moveHandle = default;
            MotionHandle rotateHandle = default;

            int actualLoops = returnToStart ? loopCount * 2 : loopCount;

            // Callback to remove from active handles when complete
            System.Action onCompleteCallback = () =>
            {
                if (activeMotionHandles.ContainsKey(transform))
                {
                    activeMotionHandles.Remove(transform);
                }
            };

            if (move)
            {
                if (useLocalSpace)
                {
                    if (isUI)
                    {
                        moveHandle = LMotion.Create(_startAnchoredPosition, _targetAnchoredPosition, Duration)
                            .WithEase(ease)
                            .WithLoops(actualLoops, loopType)
                            .WithOnComplete(onCompleteCallback)
                            .BindToAnchoredPosition(rectTransform);
                    }
                    else
                    {
                        moveHandle = LMotion.Create(_startLocalPosition, _targetLocalPosition, Duration)
                            .WithEase(ease)
                            .WithLoops(actualLoops, loopType)
                            .WithOnComplete(onCompleteCallback)
                            .BindToLocalPosition(transform);

                            Debug.Log($"Creating local position motion from {_startLocalPosition} to {_targetLocalPosition} with offset {moveOffset}");
                    }
                }
                else
                {
                    if (isUI)
                    {
                        moveHandle = LMotion.Create(_startAnchoredPosition3D, _targetAnchoredPosition3D, Duration)
                            .WithEase(ease)
                            .WithLoops(actualLoops, loopType)
                            .WithOnComplete(onCompleteCallback)
                            .BindToAnchoredPosition3D(rectTransform);
                    }
                    else
                    {
                        moveHandle = LMotion.Create(_startPosition, _targetPosition, Duration)
                            .WithEase(ease)
                            .WithLoops(actualLoops, loopType)
                            .WithOnComplete(onCompleteCallback)
                            .BindToPosition(transform);
                    }
                }
            }

            if (rotate)
            {
                if (useLocalSpace)
                {
                    rotateHandle = LMotion.Create(_startLocalRotation.eulerAngles, _targetLocalRotation, Duration)
                        .WithEase(ease)
                        .WithLoops(actualLoops, loopType)
                        .WithOnComplete(move ? null : onCompleteCallback) // Only set callback if not moving
                        .BindToLocalEulerAngles(transform);
                }
                else
                {
                    rotateHandle = LMotion.Create(_startRotation.eulerAngles, _targetRotation, Duration)
                        .WithEase(ease)
                        .WithLoops(actualLoops, loopType)
                        .WithOnComplete(move ? null : onCompleteCallback) // Only set callback if not moving
                        .BindToEulerAngles(transform);
                }
            }

            // Store the primary motion handle (prefer move over rotate if both exist)
            MotionHandle primaryHandle = move ? moveHandle : rotateHandle;
            
            if (primaryHandle.IsActive())
            {
                activeMotionHandles[transform] = primaryHandle;
            }

            return this;
        }

        public void UpdateMoveOffset(Vector3 offset)
        {
            moveOffset = offset;
        }
        
        public void UpdateRotationOffset(Vector3 offset)
        {
            rotationOffset = offset;
        }
        
        public void SetCustomStartingPosition(Vector3 position)
        {
            useStartingPosition = true;
            customStartingPosition = position;
        }
        
        public void SetCustomStartingRotation(Vector3 rotation)
        {
            useStartingRotation = true;
            customStartingRotation = rotation;
        }
        
        public void ClearCustomStartingPosition()
        {
            useStartingPosition = false;
        }
        
        public void ClearCustomStartingRotation()
        {
            useStartingRotation = false;
        }

        public void Complete(Transform transform)
        {
            if (activeMotionHandles.TryGetValue(transform, out MotionHandle existingHandle))
            {
                if (existingHandle.IsActive())
                {
                    existingHandle.Complete();
                }
                activeMotionHandles.Remove(transform);
            }
        }

        public override void Complete()
        {            
            var values = new Dictionary<Transform, MotionHandle>(activeMotionHandles);

            foreach (var kvp in values)
            {
                var handle = kvp.Value;
                if (handle.IsActive())
                {
                    handle.Complete();
                }

                activeMotionHandles.Remove(kvp.Key);
            }
        }
    }
}
