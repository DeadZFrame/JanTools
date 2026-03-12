using System;
using Jan.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace Giant.Feel
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

        [SerializeField, BoxGroup("Components"), ShowIf(nameof(useImage)), LabelText("Target Images")] 
        private Image[] targetImages;
        
        [SerializeField, BoxGroup("Components"), ShowIf(nameof(useTextMeshPro)), LabelText("Target TextMeshPro")] 
        private TextMeshProUGUI[] targetTexts;
        
        [SerializeField, BoxGroup("Components"), ShowIf(nameof(useRenderer)), LabelText("Target Renderers")] 
        private Renderer[] targetRenderers;
        
        [SerializeField, BoxGroup("Components"), ShowIf(nameof(useRenderer)), LabelText("Material Color Property")] 
        private string materialColorProperty = "_BaseColor";

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

        private Transform _targetTransform;
        private MaterialPropertyBlock[] _propertyBlocks;

        public override FeedbackBase Play(Transform transform)
        {
            if (_targetTransform != transform)
            {
                targetImages = null;
                targetTexts = null;
                targetRenderers = null;
                _targetTransform = transform;
            }

            // Auto-populate target arrays if they're empty
            if (useImage && (targetImages == null || targetImages.Length == 0))
            {
                if (targetImages == null || targetImages.Length == 0)
                {
                    targetImages = transform.GetComponentsInChildren<Image>();
                }
            }

            if (useTextMeshPro && (targetTexts == null || targetTexts.Length == 0))
            {
                if (targetTexts == null || targetTexts.Length == 0)
                {
                    targetTexts = transform.GetComponentsInChildren<TextMeshProUGUI>();
                }
            }

            if (useRenderer && (targetRenderers == null || targetRenderers.Length == 0))
            {
                if (targetRenderers == null || targetRenderers.Length == 0)
                {
                    targetRenderers = transform.GetComponentsInChildren<Renderer>();
                }
            }

            bool hasValidImageTargets = useImage && targetImages != null && targetImages.Length > 0;
            bool hasValidTextTargets = useTextMeshPro && targetTexts != null && targetTexts.Length > 0;
            bool hasValidRendererTargets = useRenderer && targetRenderers != null && targetRenderers.Length > 0;

            // Check if we have any valid targets
            if (!hasValidImageTargets && !hasValidTextTargets && !hasValidRendererTargets)
            {
                Debug.LogWarning("ColorGradient: No valid target components assigned.");
                return this; // No valid targets found
            }

            // Store default colors in local variables instead of class fields
            Color[] localDefaultTextColors = null;
            Color[] localDefaultImageColors = null;
            Color[] localDefaultRendererColors = null;

            // Initialize property blocks for renderers
            if (hasValidRendererTargets)
            {
                _propertyBlocks = new MaterialPropertyBlock[targetRenderers.Length];
                localDefaultRendererColors = new Color[targetRenderers.Length];

                for (int i = 0; i < targetRenderers.Length; i++)
                {
                    if (targetRenderers[i] != null && targetRenderers[i].material.HasProperty(materialColorProperty))
                    {
                        _propertyBlocks[i] = new MaterialPropertyBlock();
                        targetRenderers[i].GetPropertyBlock(_propertyBlocks[i]);

                        if (_propertyBlocks[i].HasColor(materialColorProperty))
                        {
                            localDefaultRendererColors[i] = _propertyBlocks[i].GetColor(materialColorProperty);
                        }
                        else
                        {
                            localDefaultRendererColors[i] = targetRenderers[i].material.GetColor(materialColorProperty);
                        }
                    }
                }
            }

            // Store default colors for text and images
            if (hasValidTextTargets)
            {
                localDefaultTextColors = new Color[targetTexts.Length];
                for (int i = 0; i < targetTexts.Length; i++)
                {
                    if (targetTexts[i] != null)
                    {
                        localDefaultTextColors[i] = targetTexts[i].color;
                    }
                }
            }

            if (hasValidImageTargets)
            {
                localDefaultImageColors = new Color[targetImages.Length];
                for (int i = 0; i < targetImages.Length; i++)
                {
                    if (targetImages[i] != null)
                    {
                        localDefaultImageColors[i] = targetImages[i].color;
                    }
                }
            }

            if (hasValidRendererTargets)
            {
                localDefaultRendererColors = new Color[targetRenderers.Length];
                for (int i = 0; i < targetRenderers.Length; i++)
                {
                    if (targetRenderers[i] != null && targetRenderers[i].material.HasProperty(materialColorProperty))
                    {
                        localDefaultRendererColors[i] = targetRenderers[i].material.GetColor(materialColorProperty);
                    }
                }
            }

            float delta = 0;
            float cachedDuration = Duration;
            bool cachedUseFullGradient = useFullGradient;
            bool cachedAffectRed = affectRed;
            bool cachedAffectGreen = affectGreen;
            bool cachedAffectBlue = affectBlue;
            bool cachedAffectAlpha = affectAlpha;
            Gradient cachedGradient = gradient;
            Gradient cachedRedGradient = redGradient;
            Gradient cachedGreenGradient = greenGradient;
            Gradient cachedBlueGradient = blueGradient;
            Gradient cachedAlphaGradient = alphaGradient;
            bool cachedUseTextMeshPro = useTextMeshPro;
            bool cachedUseImage = useImage;
            bool cachedUseRenderer = useRenderer;
            TextMeshProUGUI[] cachedTargetTexts = targetTexts;
            Image[] cachedTargetImages = targetImages;
            Renderer[] cachedTargetRenderers = targetRenderers;
            MaterialPropertyBlock[] cachedPropertyBlocks = _propertyBlocks;
            string cachedMaterialColorProperty = materialColorProperty;
            bool cachedReturnToDefaultColor = returnToDefaultColor;

            // Cache default color arrays
            Color[] cachedDefaultTextColors = localDefaultTextColors;
            Color[] cachedDefaultImageColors = localDefaultImageColors;
            Color[] cachedDefaultRendererColors = localDefaultRendererColors;

            Timed.CallWhileTrue(() => delta < cachedDuration, () =>
            {
                delta += Time.deltaTime;
                float progress = delta / cachedDuration;

                if (cachedUseTextMeshPro && cachedTargetTexts != null)
                {
                    for (int i = 0; i < cachedTargetTexts.Length; i++)
                    {
                        if (cachedTargetTexts[i] != null && i < cachedDefaultTextColors.Length)
                        {
                            Color currentColor;
                            Color startColor = cachedDefaultTextColors[i];

                            if (cachedUseFullGradient)
                            {
                                currentColor = cachedGradient.Evaluate(progress);
                            }
                            else
                            {
                                // Get original color as starting point
                                currentColor = cachedTargetTexts[i].color;

                                // Apply individual channel gradients with clamping
                                if (cachedAffectRed)
                                {
                                    float newRed = cachedRedGradient.Evaluate(progress).r;
                                    currentColor.r = Mathf.Clamp(newRed, 0, startColor.r);
                                }
                                if (cachedAffectGreen)
                                {
                                    float newGreen = cachedGreenGradient.Evaluate(progress).g;
                                    currentColor.g = Mathf.Clamp(newGreen, 0, startColor.g);
                                }
                                if (cachedAffectBlue)
                                {
                                    float newBlue = cachedBlueGradient.Evaluate(progress).b;
                                    currentColor.b = Mathf.Clamp(newBlue, 0, startColor.b);
                                }
                                if (cachedAffectAlpha)
                                {
                                    float newAlpha = cachedAlphaGradient.Evaluate(progress).a;
                                    currentColor.a = Mathf.Clamp(newAlpha, 0, startColor.a);
                                }
                            }

                            cachedTargetTexts[i].color = currentColor;
                        }
                    }
                }

                if (cachedUseImage && cachedTargetImages != null)
                {
                    for (int i = 0; i < cachedTargetImages.Length; i++)
                    {
                        if (cachedTargetImages[i] != null && i < cachedDefaultImageColors.Length)
                        {
                            Color currentColor;
                            Color startColor = cachedDefaultImageColors[i];

                            if (cachedUseFullGradient)
                            {
                                currentColor = cachedGradient.Evaluate(progress);
                            }
                            else
                            {
                                // Get original color as starting point
                                currentColor = cachedTargetImages[i].color;

                                // Apply individual channel gradients with clamping
                                if (cachedAffectRed)
                                {
                                    float newRed = cachedRedGradient.Evaluate(progress).r;
                                    currentColor.r = Mathf.Clamp(newRed, 0, startColor.r);
                                }
                                if (cachedAffectGreen)
                                {
                                    float newGreen = cachedGreenGradient.Evaluate(progress).g;
                                    currentColor.g = Mathf.Clamp(newGreen, 0, startColor.g);
                                }
                                if (cachedAffectBlue)
                                {
                                    float newBlue = cachedBlueGradient.Evaluate(progress).b;
                                    currentColor.b = Mathf.Clamp(newBlue, 0, startColor.b);
                                }
                                if (cachedAffectAlpha)
                                {
                                    float newAlpha = cachedAlphaGradient.Evaluate(progress).a;
                                    currentColor.a = Mathf.Clamp(newAlpha, 0, startColor.a);
                                }
                            }

                            cachedTargetImages[i].color = currentColor;
                        }
                    }
                }

                if (cachedUseRenderer && cachedTargetRenderers != null)
                {
                    for (int i = 0; i < cachedTargetRenderers.Length; i++)
                    {
                        if (cachedTargetRenderers[i] != null && cachedPropertyBlocks[i] != null && i < cachedDefaultRendererColors.Length)
                        {
                            Color currentColor;
                            Color startColor = cachedDefaultRendererColors[i];

                            if (cachedUseFullGradient)
                            {
                                currentColor = cachedGradient.Evaluate(progress);
                            }
                            else
                            {
                                // Get original color as starting point
                                currentColor = cachedDefaultRendererColors[i];

                                // Apply individual channel gradients with clamping
                                if (cachedAffectRed)
                                {
                                    float newRed = cachedRedGradient.Evaluate(progress).r;
                                    currentColor.r = Mathf.Clamp(newRed, 0, startColor.r);
                                }
                                if (cachedAffectGreen)
                                {
                                    float newGreen = cachedGreenGradient.Evaluate(progress).g;
                                    currentColor.g = Mathf.Clamp(newGreen, 0, startColor.g);
                                }
                                if (cachedAffectBlue)
                                {
                                    float newBlue = cachedBlueGradient.Evaluate(progress).b;
                                    currentColor.b = Mathf.Clamp(newBlue, 0, startColor.b);
                                }
                                if (cachedAffectAlpha)
                                {
                                    float newAlpha = cachedAlphaGradient.Evaluate(progress).a;
                                    currentColor.a = Mathf.Clamp(newAlpha, 0, startColor.a);
                                }
                            }

                            cachedPropertyBlocks[i].SetColor(cachedMaterialColorProperty, currentColor);
                            cachedTargetRenderers[i].SetPropertyBlock(cachedPropertyBlocks[i]);
                        }
                    }
                }

            }, transform.gameObject).OnCompleted(() =>
            {
                Timed.CallAfterTrue(() => !transform.gameObject.activeInHierarchy, () =>
                {
                    if (cachedReturnToDefaultColor)
                    {
                        if (cachedUseTextMeshPro && cachedTargetTexts != null && localDefaultTextColors != null)
                        {
                            for (int i = 0; i < cachedTargetTexts.Length && i < localDefaultTextColors.Length; i++)
                            {
                                if (cachedTargetTexts[i] != null)
                                {
                                    cachedTargetTexts[i].color = localDefaultTextColors[i];
                                }
                            }
                        }

                        if (cachedUseImage && cachedTargetImages != null && localDefaultImageColors != null)
                        {
                            for (int i = 0; i < cachedTargetImages.Length && i < localDefaultImageColors.Length; i++)
                            {
                                if (cachedTargetImages[i] != null)
                                {
                                    cachedTargetImages[i].color = localDefaultImageColors[i];
                                }
                            }
                        }

                        if (cachedUseRenderer && cachedTargetRenderers != null && localDefaultRendererColors != null)
                        {
                            for (int i = 0; i < cachedTargetRenderers.Length && i < localDefaultRendererColors.Length; i++)
                            {
                                if (cachedTargetRenderers[i] != null && cachedPropertyBlocks[i] != null)
                                {
                                    cachedPropertyBlocks[i].SetColor(cachedMaterialColorProperty, localDefaultRendererColors[i]);
                                    cachedTargetRenderers[i].SetPropertyBlock(cachedPropertyBlocks[i]);
                                }
                            }
                        }
                    }
                }, transform.gameObject);
            });

            return this;
        }

        public override void Complete()
        {
            // ColorGradient feedback uses the Timed system which doesn't provide 
            // direct cancellation functionality. The effect will complete naturally
            // based on its duration. No immediate completion action is available.
        }

        public override void Stop()
         {
             // ColorGradient feedback uses the Timed system which doesn't provide 
             // direct cancellation functionality. The effect will stop naturally
             // based on its duration. No immediate stop action is available.
         }
    }
}
