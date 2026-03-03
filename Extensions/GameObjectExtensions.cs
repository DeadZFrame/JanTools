using Jan.Core;
using UnityEngine;

namespace Jan.Core
{
    public static class GameObjectExtensions
    {
        public static bool TryGetComponentInChildren<T>(this GameObject gameObject, out T component, bool includeInactive = false)
        {
            component = gameObject.GetComponentInChildren<T>(includeInactive);
            return component != null;
        }

        public static bool TryGetComponentInParent<T>(this GameObject gameObject, out T component, bool includeInactive = false)
        {
            component = gameObject.GetComponentInParent<T>(includeInactive);
            return component != null;
        }

        public static int TryGetComponentsInChildren<T>(this GameObject gameObject, out T[] components, bool includeInactive = false)
        {
            components = gameObject.GetComponentsInChildren<T>(includeInactive);
            return components.Length;
        }
        
        public static bool TryGetComponentInParentChildren<T>(this GameObject gameObject, out T component, bool includeInactive = false)
        {
            if (gameObject.TryGetComponentInChildren(out component, includeInactive))
            {
                return true;
            }

            if (gameObject.TryGetComponentInParent(out component, includeInactive))
            {
                return true;
            }

            return false;
        }

        public static void SetLayer(this GameObject gameObject, string layer)
        {
            if(Layers.Ignore == LayerMask.LayerToName(gameObject.layer))
            {
                return;
            }
            
            var layerMask = LayerMask.NameToLayer(layer);
            if (layerMask == -1)
            {
                throw new System.Exception($"Layer '{layer}' not found.");
            }

            gameObject.layer = layerMask;
            if(gameObject.layer != layerMask)
            {
                throw new System.Exception($"Layer '{layer}' could not set.");
            }
        }

        public static void SetLayerToChildren(this GameObject gameObject, string layer)
        {
            if(Layers.Ignore == LayerMask.LayerToName(gameObject.layer))
            {
                return;
            }
            
            var layerMask = LayerMask.NameToLayer(layer);
            if (layerMask == -1)
            {
                throw new System.Exception($"Layer '{layer}' not found.");
            }

            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.SetLayer(layer);
            }

            gameObject.layer = layerMask;
            if(gameObject.layer != layerMask)
            {
                throw new System.Exception($"Layer '{layer}' could not set.");
            }
        }
    }
}


