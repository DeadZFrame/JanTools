using Jan.Core;
using Jan.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Jan.Interaction
{
    public class HighlightManager : Singleton<HighlightManager>
    {
        [SerializeField] private Material highlightMaterial, invalidHighlighMaterial; // Material used for highlighting
        //[SerializeField] private float smoothing = 0.5f; // Animation smoothing factor

        private readonly Dictionary<Renderer, Material[]> OriginalMaterials = new();

        private bool _isinvalid = false;
        private Cts _cts;

        public void Highlight(Transform target)
        {
            if(_isinvalid) return;

            if (target.gameObject.TryGetComponentInParentChildren<Renderer>(out var renderer))
            {
                // Store original materials if not already stored
                if (!OriginalMaterials.ContainsKey(renderer))
                {
                    OriginalMaterials[renderer] = renderer.sharedMaterials;
                }

                // Add highlight material as a second material
                Material[] materials = new Material[OriginalMaterials[renderer].Length + 1];
                OriginalMaterials[renderer].CopyTo(materials, 0);
                materials[^1] = highlightMaterial;
                renderer.materials = materials;
            }
        }

        public void HighlightInvalid(Transform target)
        {
            if (target.gameObject.TryGetComponentInParentChildren<Renderer>(out var renderer))
            {
                // Store original materials if not already stored
                if (!OriginalMaterials.ContainsKey(renderer))
                {
                    OriginalMaterials[renderer] = renderer.sharedMaterials;
                }

                // Add invalid highlight material as a second material
                Material[] materials = new Material[OriginalMaterials[renderer].Length + 1];
                OriginalMaterials[renderer].CopyTo(materials, 0);
                materials[^1] = invalidHighlighMaterial;
                renderer.materials = materials;
            }

            var duration = .5f;
            Timed.CallDelayed(duration, () => Unhighlight(target));
            _cts?.Cancel();
            _cts = Timed.CallDelayed(duration, () => _isinvalid = false);
            _isinvalid = true;
        }

        public void Unhighlight(Transform target)
        {
            if(_isinvalid) return;
            if (target.gameObject.TryGetComponentInParentChildren<Renderer>(out var renderer) && OriginalMaterials.ContainsKey(renderer))
            {
                // Restore original materials
                renderer.materials = OriginalMaterials[renderer];
                OriginalMaterials.Remove(renderer);
            }
        }
    }
}
