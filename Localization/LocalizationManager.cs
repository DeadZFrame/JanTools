using System.Collections.Generic;
using Jan.Core;
using Jan.Events;
using UnityEngine;

namespace Jan.Localization
{
    public static class LocalizationManager
    {
        private static Dictionary<string, LocalizationItem[]> localizedText;

        public static void LoadLanguage(string langCode)
        {
            localizedText = new Dictionary<string, LocalizationItem[]>();

            // Load JSON file from Assets/Resources/Locales/
            TextAsset jsonAsset = Resources.Load<TextAsset>($"Locales/{langCode}");

            if (jsonAsset != null)
            {
                LocalizationData data = JsonUtility.FromJson<LocalizationData>(jsonAsset.text);

                foreach (LocalizationContext cls in data.items)
                {
                    localizedText[cls.key] = cls.value;
                }

                Debug.Log($"Language loaded: {langCode}");
                EventManager.Trigger(EventNames.OnLanguageLoaded);
            }
            else
            {
                Debug.LogError($"Localization file not found for language: {langCode}");
            }
        }

        public static string GetLocalizedValue(string contextKey, string key)
        {
            if(localizedText == null)
            {
                Debug.LogWarning("LocalizationManager: No language loaded. Please call LoadLanguage first.");
                return null;
            }

            if (localizedText.TryGetValue(contextKey, out LocalizationItem[] items))
            {
                if(items.TryGetMatch(item => item.key.Equals(key), out LocalizationItem foundItem))
                {
                    return foundItem.value;
                }
            }

            return $"[{key}]"; // Returns the key as a fallback if missing
        }

        public static string[] GetContextNames()
        {
            if(localizedText == null)
            {
                Debug.LogWarning("LocalizationManager: No language loaded. Please call LoadLanguage first.");
                return new string[0];
            }

            string[] contextNames = new string[localizedText.Count];
            localizedText.Keys.CopyTo(contextNames, 0);
            return contextNames;
        }

        public static string[] GetContext(string contextKey)
        {
            if(localizedText == null)
            {
                Debug.LogWarning("LocalizationManager: No language loaded. Please call LoadLanguage first.");
                return new string[0];
            }
            
            if (localizedText.TryGetValue(contextKey, out LocalizationItem[] items))
            {
                string[] values = new string[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    values[i] = items[i].key;
                }
                return values;
            }

            Debug.LogWarning($"Localization context not found: {contextKey}");

            return new string[0]; // Returns an empty array if the context is not found
        }
    }

}

