using System;
using System.Collections.Generic;
using Jan.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace Jan.Feel
{
    [Serializable]
    public class ColorGradient : FeedbackBase
    {
        [SerializeField, BoxGroup("Components"), LabelText("Use Image")]
        private bool useImage = true;

        [SerializeField, BoxGroup("Components"), LabelText("Use TextMeshPro")]
        private bool useTextMeshPro;

        [SerializeField, BoxGroup("Components"), LabelText("Use Renderer")]
        private bool useRenderer;

        [SerializeField, BoxGroup("Components"), LabelText("Use Canvas Group")]
        private bool useCanvasGroup;

        [SerializeField, BoxGroup("Components"), ShowIf(nameof(useImage)), LabelText("Target Images")]
        private Image[] targetImages;

        [SerializeField, BoxGroup("Components"), ShowIf(nameof(useTextMeshPro)), LabelText("Target TextMeshPro")]
        private TextMeshProUGUI[] targetTexts;

        [SerializeField, BoxGroup("Components"), ShowIf(nameof(useRenderer)), LabelText("Target Renderers")]
        private Renderer[] targetRenderers;

        [SerializeField, BoxGroup("Components"), ShowIf(nameof(useCanvasGroup)), LabelText("Target Canvas Groups")]
        private CanvasGroup[] targetCanvasGroups;

        [SerializeField, BoxGroup("Components"), ShowIf(nameof(useRenderer)), LabelText("Material Color Property")]
        private string materialColorProperty = "_BaseColor";

        [SerializeField, BoxGroup("Components")]
        private bool autoGetComponents;

        [BoxGroup("Components"), Button("Get Images in Children"), ShowIf(nameof(useImage))]
        private void GetImagesInChildren(Transform transform)
        {
            if (transform != null)
            {
                targetImages = transform.GetComponentsInChildren<Image>();
            }
        }

        [BoxGroup("Components"), Button("Get TextMeshPros in Children"), ShowIf(nameof(useTextMeshPro))]
        private void GetTextMeshProsInChildren(Transform transform)
        {
            if (transform != null)
            {
                targetTexts = transform.GetComponentsInChildren<TextMeshProUGUI>();
            }
        }

        [BoxGroup("Components"), Button("Get Renderers in Children"), ShowIf(nameof(useRenderer))]
        private void GetRenderersInChildren(Transform transform)
        {
            if (transform != null)
            {
                targetRenderers = transform.GetComponentsInChildren<Renderer>();
            }
        }

        [BoxGroup("Components"), Button("Get Canvas Groups in Children"), ShowIf(nameof(useCanvasGroup))]
        private void GetCanvasGroupsInChildren(Transform transform)
        {
            if (transform != null)
            {
                targetCanvasGroups = transform.GetComponentsInChildren<CanvasGroup>();
            }
        }

        [SerializeField, FoldoutGroup("Settings")]
        private bool useFullGradient = true;

        [SerializeField, FoldoutGroup("Settings"), ShowIf(nameof(useFullGradient))]
        private Gradient gradient;

        [SerializeField, FoldoutGroup("Settings"), HideIf(nameof(useFullGradient))]
        private bool affectRed;

        [SerializeField, FoldoutGroup("Settings"), HideIf(nameof(useFullGradient)), ShowIf(nameof(affectRed))]
        private Gradient redGradient;

        [SerializeField, FoldoutGroup("Settings"), HideIf(nameof(useFullGradient))]
        private bool affectGreen;

        [SerializeField, FoldoutGroup("Settings"), HideIf(nameof(useFullGradient)), ShowIf(nameof(affectGreen))]
        private Gradient greenGradient;

        [SerializeField, FoldoutGroup("Settings"), HideIf(nameof(useFullGradient))]
        private bool affectBlue;

        [SerializeField, FoldoutGroup("Settings"), HideIf(nameof(useFullGradient)), ShowIf(nameof(affectBlue))]
        private Gradient blueGradient;

        [SerializeField, FoldoutGroup("Settings"), HideIf(nameof(useFullGradient))]
        private bool affectAlpha;

        [SerializeField, FoldoutGroup("Settings"), HideIf(nameof(useFullGradient)), ShowIf(nameof(affectAlpha))]
        private Gradient alphaGradient;

        [SerializeField, FoldoutGroup("Settings")]
        private bool returnToDefaultColor;

        [SerializeField, FoldoutGroup("Settings")]
        private bool loop;

        [SerializeField, FoldoutGroup("Settings"), ShowIf(nameof(loop)), Tooltip("Number of times to loop. Values <= 0 loop indefinitely.")]
        private int loopCount = -1;

        [SerializeField, FoldoutGroup("Settings"), ShowIf(nameof(loop)), Tooltip("If true, the gradient reverses direction each loop instead of restarting from the beginning.")]
        private bool pingPong;

        private Transform _targetTransform;

        // Active runs, keyed by the transform they were started on, so Complete/Stop can end them
        private readonly Dictionary<Transform, Run> _activeRuns = new Dictionary<Transform, Run>();

        private class Run
        {
            public Cts Cts;
            public Action<bool> Finish;
            public bool Finished;
        }

        public override FeedbackBase Play(Transform transform)
        {
            if (transform == null)
            {
                Debug.LogWarning("ColorGradient: Play called without a transform.");
                return this;
            }

            if (autoGetComponents && _targetTransform != transform)
            {
                targetImages = null;
                targetTexts = null;
                targetRenderers = null;
                targetCanvasGroups = null;
                _targetTransform = transform;
            }

            if (autoGetComponents)
            {
                // Auto-populate target arrays if they're empty
                if (useImage && (targetImages == null || targetImages.Length == 0))
                {
                    targetImages = transform.GetComponentsInChildren<Image>();
                }

                if (useTextMeshPro && (targetTexts == null || targetTexts.Length == 0))
                {
                    targetTexts = transform.GetComponentsInChildren<TextMeshProUGUI>();
                }

                if (useRenderer && (targetRenderers == null || targetRenderers.Length == 0))
                {
                    targetRenderers = transform.GetComponentsInChildren<Renderer>();
                }

                if (useCanvasGroup && (targetCanvasGroups == null || targetCanvasGroups.Length == 0))
                {
                    targetCanvasGroups = transform.GetComponentsInChildren<CanvasGroup>();
                }
            }

            bool hasValidImageTargets = useImage && targetImages != null && targetImages.Length > 0;
            bool hasValidTextTargets = useTextMeshPro && targetTexts != null && targetTexts.Length > 0;
            bool hasValidRendererTargets = useRenderer && targetRenderers != null && targetRenderers.Length > 0;
            bool hasValidCanvasGroupTargets = useCanvasGroup && targetCanvasGroups != null && targetCanvasGroups.Length > 0;

            // Check if we have any valid targets
            if (!hasValidImageTargets && !hasValidTextTargets && !hasValidRendererTargets && !hasValidCanvasGroupTargets)
            {
                Debug.LogWarning("ColorGradient: No valid target components assigned.");
                return this;
            }

            // Cache everything the running task needs, so inspector edits can't change a run in flight
            bool cachedUseFullGradient = useFullGradient;
            Gradient cachedGradient = gradient;
            Gradient cachedRedGradient = redGradient;
            Gradient cachedGreenGradient = greenGradient;
            Gradient cachedBlueGradient = blueGradient;
            Gradient cachedAlphaGradient = alphaGradient;

            // A channel is applied only when it is enabled and actually has a gradient to evaluate
            bool applyRed = !cachedUseFullGradient && affectRed && cachedRedGradient != null;
            bool applyGreen = !cachedUseFullGradient && affectGreen && cachedGreenGradient != null;
            bool applyBlue = !cachedUseFullGradient && affectBlue && cachedBlueGradient != null;
            bool applyAlpha = !cachedUseFullGradient && affectAlpha && cachedAlphaGradient != null;

            if (cachedUseFullGradient && cachedGradient == null)
            {
                Debug.LogWarning("ColorGradient: No gradient assigned.");
                return this;
            }

            if (!cachedUseFullGradient && !applyRed && !applyGreen && !applyBlue && !applyAlpha)
            {
                Debug.LogWarning("ColorGradient: No channel gradients assigned.");
                return this;
            }

            // A previous run on the same transform would fight this one over the same colors
            if (_activeRuns.TryGetValue(transform, out var previousRun))
            {
                previousRun.Finish?.Invoke(false);
            }

            Color[] defaultTextColors = null;
            Color[] defaultImageColors = null;
            Color[] defaultRendererColors = null;
            float[] defaultCanvasGroupAlphas = null;
            MaterialPropertyBlock[] propertyBlocks = null;

            if (hasValidTextTargets)
            {
                defaultTextColors = new Color[targetTexts.Length];
                for (int i = 0; i < targetTexts.Length; i++)
                {
                    if (targetTexts[i] != null)
                    {
                        defaultTextColors[i] = targetTexts[i].color;
                    }
                }
            }

            if (hasValidImageTargets)
            {
                defaultImageColors = new Color[targetImages.Length];
                for (int i = 0; i < targetImages.Length; i++)
                {
                    if (targetImages[i] != null)
                    {
                        defaultImageColors[i] = targetImages[i].color;
                    }
                }
            }

            if (hasValidCanvasGroupTargets)
            {
                defaultCanvasGroupAlphas = new float[targetCanvasGroups.Length];
                for (int i = 0; i < targetCanvasGroups.Length; i++)
                {
                    if (targetCanvasGroups[i] != null)
                    {
                        defaultCanvasGroupAlphas[i] = targetCanvasGroups[i].alpha;
                    }
                }
            }

            // The property block doubles as the "this renderer is usable" marker: a renderer with no
            // material, or whose material lacks the color property, is left with a null block.
            if (hasValidRendererTargets)
            {
                propertyBlocks = new MaterialPropertyBlock[targetRenderers.Length];
                defaultRendererColors = new Color[targetRenderers.Length];

                for (int i = 0; i < targetRenderers.Length; i++)
                {
                    var targetRenderer = targetRenderers[i];
                    if (targetRenderer == null) continue;

                    var material = targetRenderer.sharedMaterial;
                    if (material == null || !material.HasProperty(materialColorProperty)) continue;

                    var block = new MaterialPropertyBlock();
                    targetRenderer.GetPropertyBlock(block);
                    propertyBlocks[i] = block;

                    defaultRendererColors[i] = block.HasColor(materialColorProperty)
                        ? block.GetColor(materialColorProperty)
                        : material.GetColor(materialColorProperty);
                }
            }

            float cachedDuration = Mathf.Max(Duration, 0f);
            bool cachedUseTextMeshPro = hasValidTextTargets;
            bool cachedUseImage = hasValidImageTargets;
            bool cachedUseRenderer = hasValidRendererTargets;
            bool cachedUseCanvasGroup = hasValidCanvasGroupTargets;
            TextMeshProUGUI[] cachedTargetTexts = targetTexts;
            Image[] cachedTargetImages = targetImages;
            Renderer[] cachedTargetRenderers = targetRenderers;
            CanvasGroup[] cachedTargetCanvasGroups = targetCanvasGroups;
            string cachedMaterialColorProperty = materialColorProperty;
            bool cachedReturnToDefaultColor = returnToDefaultColor;
            bool cachedLoop = loop;
            int cachedLoopCount = loopCount;
            bool cachedPingPong = pingPong;

            // Where the gradient lands when it ends: one full pass, or the end of the last loop
            float endProgress = !cachedLoop
                ? 1f
                : cachedPingPong
                    ? Mathf.PingPong(cachedLoopCount, 1f)
                    : Mathf.Repeat(cachedLoopCount, 1f);

            // A zero-length gradient has no curve to walk, so just land on its end value
            if (cachedDuration <= 0f)
            {
                ApplyProgress(endProgress);
                if (cachedReturnToDefaultColor) RestoreDefaults();
                return this;
            }

            float delta = 0f;
            var run = new Run();

            run.Finish = applyEndValue =>
            {
                if (run.Finished) return;
                run.Finished = true;

                RemoveRun(transform, run);
                run.Cts?.SafeCancel();

                if (applyEndValue) ApplyProgress(endProgress);
                if (cachedReturnToDefaultColor) RestoreDefaults();
            };

            run.Cts = Timed.CallWhileTrue(() =>
            {
                if (!cachedLoop) return delta < cachedDuration;
                if (cachedLoopCount <= 0) return true;
                return delta / cachedDuration < cachedLoopCount;
            }, () =>
            {
                // Apply first, then advance, so the gradient starts at its 0 position on the first frame
                ApplyProgress(GetProgress(delta));
                delta += Time.deltaTime;
            }, transform.gameObject);

            run.Cts.OnCompleted(() => run.Finish(true));
            // Cancelled means the object was destroyed, or Stop ran, so don't touch the targets here
            run.Cts.OnCancelled(() =>
            {
                run.Finished = true;
                RemoveRun(transform, run);
            });

            _activeRuns[transform] = run;

            return this;

            float GetProgress(float elapsed)
            {
                float rawProgress = elapsed / cachedDuration;

                return !cachedLoop
                    ? Mathf.Clamp01(rawProgress)
                    : cachedPingPong
                        ? Mathf.PingPong(rawProgress, 1f)
                        : Mathf.Repeat(rawProgress, 1f);
            }

            // Builds the color for one target: either straight from the full gradient, or by
            // layering the enabled channel gradients over that target's default color.
            Color EvaluateColor(float progress, Color startColor)
            {
                if (cachedUseFullGradient) return cachedGradient.Evaluate(progress);

                Color currentColor = startColor;

                if (applyRed)
                {
                    currentColor.r = Mathf.Clamp(cachedRedGradient.Evaluate(progress).r, 0, startColor.r);
                }
                if (applyGreen)
                {
                    currentColor.g = Mathf.Clamp(cachedGreenGradient.Evaluate(progress).g, 0, startColor.g);
                }
                if (applyBlue)
                {
                    currentColor.b = Mathf.Clamp(cachedBlueGradient.Evaluate(progress).b, 0, startColor.b);
                }
                if (applyAlpha)
                {
                    currentColor.a = Mathf.Clamp(cachedAlphaGradient.Evaluate(progress).a, 0, startColor.a);
                }

                return currentColor;
            }

            void ApplyProgress(float progress)
            {
                if (cachedUseTextMeshPro)
                {
                    for (int i = 0; i < cachedTargetTexts.Length; i++)
                    {
                        if (cachedTargetTexts[i] == null) continue;
                        cachedTargetTexts[i].color = EvaluateColor(progress, defaultTextColors[i]);
                    }
                }

                if (cachedUseImage)
                {
                    for (int i = 0; i < cachedTargetImages.Length; i++)
                    {
                        if (cachedTargetImages[i] == null) continue;
                        cachedTargetImages[i].color = EvaluateColor(progress, defaultImageColors[i]);
                    }
                }

                if (cachedUseRenderer)
                {
                    for (int i = 0; i < cachedTargetRenderers.Length; i++)
                    {
                        if (cachedTargetRenderers[i] == null || propertyBlocks[i] == null) continue;

                        propertyBlocks[i].SetColor(cachedMaterialColorProperty, EvaluateColor(progress, defaultRendererColors[i]));
                        cachedTargetRenderers[i].SetPropertyBlock(propertyBlocks[i]);
                    }
                }

                if (cachedUseCanvasGroup && (cachedUseFullGradient || applyAlpha))
                {
                    float currentAlpha = cachedUseFullGradient
                        ? cachedGradient.Evaluate(progress).a
                        : cachedAlphaGradient.Evaluate(progress).a;

                    for (int i = 0; i < cachedTargetCanvasGroups.Length; i++)
                    {
                        if (cachedTargetCanvasGroups[i] == null) continue;
                        cachedTargetCanvasGroups[i].alpha = currentAlpha;
                    }
                }
            }

            void RestoreDefaults()
            {
                if (cachedUseTextMeshPro)
                {
                    for (int i = 0; i < cachedTargetTexts.Length; i++)
                    {
                        if (cachedTargetTexts[i] == null) continue;
                        cachedTargetTexts[i].color = defaultTextColors[i];
                    }
                }

                if (cachedUseImage)
                {
                    for (int i = 0; i < cachedTargetImages.Length; i++)
                    {
                        if (cachedTargetImages[i] == null) continue;
                        cachedTargetImages[i].color = defaultImageColors[i];
                    }
                }

                if (cachedUseRenderer)
                {
                    for (int i = 0; i < cachedTargetRenderers.Length; i++)
                    {
                        if (cachedTargetRenderers[i] == null || propertyBlocks[i] == null) continue;

                        propertyBlocks[i].SetColor(cachedMaterialColorProperty, defaultRendererColors[i]);
                        cachedTargetRenderers[i].SetPropertyBlock(propertyBlocks[i]);
                    }
                }

                if (cachedUseCanvasGroup)
                {
                    for (int i = 0; i < cachedTargetCanvasGroups.Length; i++)
                    {
                        if (cachedTargetCanvasGroups[i] == null) continue;
                        cachedTargetCanvasGroups[i].alpha = defaultCanvasGroupAlphas[i];
                    }
                }
            }
        }

        // Only clears the entry if it still belongs to this run: a replaced run finishes late,
        // after the run that replaced it has already registered itself under the same transform.
        private void RemoveRun(Transform transform, Run run)
        {
            if (_activeRuns.TryGetValue(transform, out var activeRun) && activeRun == run)
            {
                _activeRuns.Remove(transform);
            }
        }

        /// <summary>
        /// Jumps every running gradient to its end value, then back to the default color when
        /// Return To Default Color is set.
        /// </summary>
        public override void Complete()
        {
            FinishActiveRuns(true);
        }

        /// <summary>
        /// Stops every running gradient where it is, leaving the current color in place unless
        /// Return To Default Color is set.
        /// </summary>
        public override void Stop()
        {
            FinishActiveRuns(false);
        }

        private void FinishActiveRuns(bool applyEndValue)
        {
            if (_activeRuns.Count == 0) return;

            var runs = new List<Run>(_activeRuns.Values);
            foreach (var run in runs)
            {
                run.Finish?.Invoke(applyEndValue);
            }

            _activeRuns.Clear();
        }
    }
}
