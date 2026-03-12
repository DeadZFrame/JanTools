using System.Collections.Generic;
using Jan.Core;
using UnityEngine;

namespace UIBus
{
    public class UIBusManager : Singleton<UIBusManager>
    {
        public Dictionary<string, IUIElement> UIElements { get; private set; } = new Dictionary<string, IUIElement>();

        public void RegisterUIElement(string key, IUIElement element)
        {
            if (!UIElements.ContainsKey(key))
            {
                UIElements.Add(key, element);
            }
            else
            {
                Debug.LogWarning($"UI Element with key '{key}' is already registered.");
            }
        }

        public bool TryGetUIElement<T>(string key, out T element) where T : IUIElement
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
    }
}