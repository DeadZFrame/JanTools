using System;
using System.Collections.Generic;
using LitMotion;
using LitMotion.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Giant.Feel
{
    [Serializable]
    public class SquashStretch : FeedbackBase
    {
        // Static dictionary to track active motion handles by transform
        private static Dictionary<Transform, MotionHandle> activeMotionHandles = new Dictionary<Transform, MotionHandle>();

        [Tooltip("Target scale to reach during the squash/stretch animation.")]
        [SerializeField] private Vector3 scale = Vector3.one;
        [Tooltip("Vibrato controls the oscillation/frequency of the punch animation. Must be >= 0.")]
        [SerializeField, ValidateInput(nameof(IsPositiveVibrato), "Value must be positive")]
        private int vibrato = 1;
        [Tooltip("Elasticity controls the springiness of the punch animation. Must be >= 0.")]
        [SerializeField, ValidateInput(nameof(IsPositiveElasticity), "Value must be positive")]
        private float elasticity = 1f;
        [Tooltip("Easing function used for the scaling animation.")]
        [SerializeField] private Ease ease = Ease.Linear;
        [Tooltip("When enabled, performs a punch (bouncy) scale animation; otherwise a smooth scale animation.")]
        [SerializeField] private bool punchScale = true;
        [Tooltip("Type of loop applied to the animation (e.g., Yoyo).")]
        [SerializeField] private LoopType loopType = LoopType.Yoyo;
        [Tooltip("How many times the animation should loop.")]
        [SerializeField] private int loopCount = 1;
        [Tooltip("If true and not using punchScale, the object will return to its initial scale after animation.")]
        [SerializeField, HideIf(nameof(punchScale))] private bool returnToInitialScale;

        private bool IsPositiveVibrato => vibrato >= 0;
        private bool IsPositiveElasticity => elasticity >= 0;

        public override FeedbackBase Play(Transform transform)
        {
            // Cancel existing motion if there's one already playing on this transform
            if (activeMotionHandles.TryGetValue(transform, out MotionHandle existingHandle) && existingHandle.IsActive())
            {
                existingHandle.Complete();
                activeMotionHandles.Remove(transform);
            }

            // Cache all variables
            var temp = transform.localScale;
            var cachedStartingScale = transform.localScale;
            var cachedScale = scale;
            var cachedDuration = Duration;
            var cachedEase = ease;
            var cachedLoopCount = loopCount;
            var cachedLoopType = loopType;
            var cachedVibrato = vibrato;
            var cachedElasticity = elasticity;
            var cachedPunchScale = punchScale;
            var cachedReturnToInitialScale = returnToInitialScale;

            transform.localScale = cachedStartingScale;

            MotionHandle handle = default;

            // Callback to remove from active handles when complete
            System.Action onCompleteCallback = () =>
            {
                if (activeMotionHandles.ContainsKey(transform))
                {
                    activeMotionHandles.Remove(transform);
                }
            };

            if (cachedPunchScale)
            {
                if (cachedStartingScale != Vector3.one)
                {
                    // First animate to original scale, then punch
                    var returnHandle = LMotion.Create(cachedStartingScale, temp, cachedDuration / 2)
                        .WithEase(cachedEase)
                        .WithLoops(cachedLoopCount, cachedLoopType)
                        .WithOnComplete(() =>
                        {
                            // After returning to original scale, do the punch
                            var punchHandle = LMotion.Punch.Create(temp, cachedScale - Vector3.one, cachedDuration)
                                .WithEase(cachedEase)
                                .WithFrequency(cachedVibrato)
                                .WithDampingRatio(cachedElasticity)
                                .WithOnComplete(onCompleteCallback)
                                .BindToLocalScale(transform);
                            
                            if (punchHandle.IsActive())
                            {
                                activeMotionHandles[transform] = punchHandle;
                            }
                        })
                        .BindToLocalScale(transform);
                    
                    handle = returnHandle;
                }
                else
                {
                    handle = LMotion.Punch.Create(cachedStartingScale, cachedScale - Vector3.one, cachedDuration)
                        .WithEase(cachedEase)
                        .WithFrequency(cachedVibrato)
                        .WithDampingRatio(cachedElasticity)
                        .WithOnComplete(onCompleteCallback)
                        .BindToLocalScale(transform);
                }
            }
            else
            {
                if (cachedReturnToInitialScale)
                {
                    handle = LMotion.Create(cachedStartingScale, cachedScale, cachedDuration)
                        .WithEase(cachedEase)
                        .WithLoops(cachedLoopCount, cachedLoopType)
                        .WithOnComplete(() =>
                        {
                            transform.localScale = temp;
                            onCompleteCallback();
                        })
                        .BindToLocalScale(transform);
                }
                else
                {
                    handle = LMotion.Create(cachedStartingScale, cachedScale, cachedDuration)
                        .WithEase(cachedEase)
                        .WithLoops(cachedLoopCount, cachedLoopType)
                        .WithOnComplete(onCompleteCallback)
                        .BindToLocalScale(transform);
                }
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