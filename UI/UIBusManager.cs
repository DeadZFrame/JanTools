using System.Collections.Generic;
using Jan.Core;
using UnityEngine;

namespace Jan.UI
{
    public static class UIBusManager
    {
        private static Dictionary<string, IUIElement> UIElements { get; } = new Dictionary<string, IUIElement>();

        public static void RegisterUIElement(string key, IUIElement element)
        {
            key = key == UINames.None ? element.GetType().Name : key;
            if (!UIElements.ContainsKey(key))
            {
                UIElements.Add(key, element);
            }
            else
            {
                Debug.LogWarning($"UI Element with key '{key}' is already registered.");
            }
        }

        public static void UnregisterUIElement(string key, IUIElement element)
        {
            key = key == UINames.None ? element.GetType().Name : key;

            if (UIElements.ContainsKey(key))
            {
                UIElements.Remove(key);
            }
            else
            {
                Debug.LogWarning($"UI Element with key '{key}' is not registered.");
            }
        }

        public static bool TryGetUIElement<T>(string key, out T element) where T : IUIElement
        {
            if (UIElements.TryGetValue(key, out var uiElement) && uiElement is T typedElement)
            {
                element = typedElement;
                return true;
            }
            else
            {
                Debug.LogError($"UI Element with key '{key}' not found or of incorrect type.");
                element = default;
                return false;
            }
        }

        public static bool TryGetUIElement<T>(out T element) where T : IUIElement
        {
            foreach (var uiElement in UIElements.Values)
            {
                if (uiElement is T typedElement)
                {
                    element = typedElement;
                    return true;
                }
            }

            Debug.LogError($"UI Element of type '{typeof(T).Name}' not found.");
            element = default;
            return false;
        }
    }
}