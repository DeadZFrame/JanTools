using System;
using System.Collections.Generic;
using LitMotion;
using LitMotion.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Giant.Feel
{
    [Serializable]
    public class ObjectShake : FeedbackBase
    {
        // Static dictionary to track active motion handles by transform
        private static Dictionary<Transform, MotionHandle> activeMotionHandles = new Dictionary<Transform, MotionHandle>();

        [SerializeField] private ShakeDirections direction;
        [SerializeField, ValidateInput(nameof(IsPositivePower), "Value must be positive")] 
        private float power = 1;
        [SerializeField, ValidateInput(nameof(IsPositiveVibrato), "Value must be positive")]
        private int vibrato = 10;
        [SerializeField, ValidateInput(nameof(IsPositiveRandomness), "Value must be positive")]
        private float randomness = 90;
        [SerializeField]
        private Ease ease = Ease.Linear;
        [SerializeField] 
        private bool fadeout = true;

        private bool IsPositivePower => power >= 0;
        private bool IsPositiveVibrato => vibrato >= 0;
        private bool IsPositiveRandomness => randomness >= 0;
        
        public enum ShakeDirections
        {
            DefaultRotation, DefaultPosition, Vertical, Horizontal
        }

        public override FeedbackBase Play(Transform transform)
        {
            // Cancel existing motion if there's one already playing on this transform
            if (activeMotionHandles.TryGetValue(transform, out MotionHandle existingHandle) && existingHandle.IsActive())
            {
                existingHandle.Complete();
                activeMotionHandles.Remove(transform);
            }

            // Cache variables
            float duration = Duration;
            float shakePower = power;
            int shakeVibrato = vibrato;
            float shakeRandomness = randomness;
            bool shakeFadeout = fadeout;
            Ease shakeEase = ease;

            // Check if transform is a RectTransform (UI element)
            RectTransform rectTransform = transform as RectTransform;
            bool isUI = rectTransform != null;

            // Pre-create common vectors
            Vector3 uiRotation = new Vector3(0f, shakePower, 0f);
            Vector3 horizontalShake = new Vector3(shakePower, 0f, shakePower);
            Vector2 uiHorizontalShake = new Vector2(shakePower, 0f);
            Vector2 uiVerticalShake = new Vector2(0f, shakePower);
            Vector3 verticalShake = new Vector3(0f, shakePower, 0f);

            MotionHandle handle = default;

            // Callback to remove from active handles when complete
            System.Action onCompleteCallback = () =>
            {
                if (activeMotionHandles.ContainsKey(transform))
                {
                    activeMotionHandles.Remove(transform);
                }
            };

            switch (direction)
            {
                case ShakeDirections.DefaultRotation:
                    if (isUI)
                    {
                        handle = LMotion.Punch.Create(Vector3.zero, uiRotation, duration)
                            .WithEase(shakeEase)
                            .WithFrequency(shakeVibrato)
                            .WithDampingRatio(1f - (shakeRandomness / 100f))
                            .WithOnComplete(onCompleteCallback)
                            .BindToLocalEulerAngles(transform);
                    }
                    else
                    {
                        handle = LMotion.Punch.Create(Vector3.zero, Vector3.one * shakePower, duration)
                            .WithEase(shakeEase)
                            .WithFrequency(shakeVibrato)
                            .WithDampingRatio(1f - (shakeRandomness / 100f))
                            .WithOnComplete(onCompleteCallback)
                            .BindToLocalEulerAngles(transform);
                    }
                    break;

                case ShakeDirections.DefaultPosition:
                    if (isUI)
                    {
                        handle = LMotion.Punch.Create(Vector2.zero, Vector2.one * shakePower, duration)
                            .WithEase(shakeEase)
                            .WithFrequency(shakeVibrato)
                            .WithDampingRatio(1f - (shakeRandomness / 100f))
                            .WithOnComplete(onCompleteCallback)
                            .BindToAnchoredPosition(rectTransform);
                    }
                    else
                    {
                        handle = LMotion.Punch.Create(Vector3.zero, Vector3.one * shakePower, duration)
                            .WithEase(shakeEase)
                            .WithFrequency(shakeVibrato)
                            .WithDampingRatio(1f - (shakeRandomness / 100f))
                            .WithOnComplete(onCompleteCallback)
                            .BindToLocalPosition(transform);
                    }
                    break;

                case ShakeDirections.Horizontal:
                    if (isUI)
                    {
                        handle = LMotion.Punch.Create(Vector2.zero, uiHorizontalShake, duration)
                            .WithEase(shakeEase)
                            .WithFrequency(shakeVibrato)
                            .WithDampingRatio(1f - (shakeRandomness / 100f))
                            .WithOnComplete(onCompleteCallback)
                            .BindToAnchoredPosition(rectTransform);
                    }
                    else
                    {
                        handle = LMotion.Punch.Create(Vector3.zero, horizontalShake, duration)
                            .WithEase(shakeEase)
                            .WithFrequency(shakeVibrato)
                            .WithDampingRatio(1f - (shakeRandomness / 100f))
                            .WithOnComplete(onCompleteCallback)
                            .BindToLocalPosition(transform);
                    }
                    break;

                case ShakeDirections.Vertical:
                    if (isUI)
                    {
                        handle = LMotion.Punch.Create(Vector2.zero, uiVerticalShake, duration)
                            .WithEase(shakeEase)
                            .WithFrequency(shakeVibrato)
                            .WithDampingRatio(1f - (shakeRandomness / 100f))
                            .WithOnComplete(onCompleteCallback)
                            .BindToAnchoredPosition(rectTransform);
                    }
                    else
                    {
                        handle = LMotion.Punch.Create(Vector3.zero, verticalShake, duration)
                            .WithEase(shakeEase)
                            .WithFrequency(shakeVibrato)
                            .WithDampingRatio(1f - (shakeRandomness / 100f))
                            .WithOnComplete(onCompleteCallback)
                            .BindToLocalPosition(transform);
                    }
                    break;
            }

            // Cache the motion handle
            if (handle.IsActive())
            {
                activeMotionHandles[transform] = handle;
            }
            
            return this;
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

        public override void Stop()
        {
            var values = new Dictionary<Transform, MotionHandle>(activeMotionHandles);

            foreach (var kvp in values)
            {
                var handle = kvp.Value;
                if (handle.IsActive())
                {
                    handle.Cancel();
                }
                
                activeMotionHandles.Remove(kvp.Key);
            }
        }
    }
}