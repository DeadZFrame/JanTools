using Jan.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Jan.InteractionSystem
{
    public class HighlightManager : Singleton<HighlightManager>
    {
        public Texture2D defaultCursor;
        public Texture2D horizontalHoverCursor;
        public Texture2D verticalHoverCursor;
        public Vector2 cursorHotspot = Vector2.zero;
        [SerializeField] private Material highlightMaterial; // Material used for highlighting
        //[SerializeField] private float smoothing = 0.5f; // Animation smoothing factor

        private readonly Dictionary<Renderer, Material[]> OriginalMaterials = new();

        public void Start()
        {
            Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);
        }

        public void Highlight(Transform target)
        {
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

        public void Unhighlight(Transform target)
        {
            if (target.gameObject.TryGetComponentInParentChildren<Renderer>(out var renderer) && OriginalMaterials.ContainsKey(renderer))
            {
                // Restore original materials
                renderer.materials = OriginalMaterials[renderer];
                OriginalMaterials.Remove(renderer);
            }
        }
    }
}
