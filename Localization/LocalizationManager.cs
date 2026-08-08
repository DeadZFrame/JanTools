using System.Collections.Generic;
using Jan.Core;
using UnityEngine;

namespace Jan.Localizaton
{
    public class LocalizationManager : Singleton<LocalizationManager>
    {
        private Dictionary<string, string> localizedText;

        private void Awake()
        {            
            // Load default language on start
            LoadLanguage("en"); 
        }

        public void LoadLanguage(string langCode)
        {
            localizedText = new Dictionary<string, string>();

            // Load JSON file from Assets/Resources/
            TextAsset jsonAsset = Resources.Load<TextAsset>(langCode);
            
            if (jsonAsset != null)
            {
                LocalizationData data = JsonUtility.FromJson<LocalizationData>(jsonAsset.text);

                foreach (LocalizationItem item in data.items)
                {
                    localizedText[item.key] = item.value;
                }
                Debug.Log($"Language loaded: {langCode}");
            }
            else
            {
                Debug.LogError($"Localization file not found for language: {langCode}");
            }
        }

        public string GetLocalizedValue(string key)
        {
            if (localizedText != null && localizedText.ContainsKey(key))
            {
                return localizedText[key];
            }
            return $"[{key}]"; // Returns the key as a fallback if missing
        }
    }

}

