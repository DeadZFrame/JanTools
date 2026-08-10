using Jan.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Jan.Localization
{
    [ExecuteAlways]
    public class LocalizationAgent : MonoBehaviour
    {
        [SerializeField, ValueDropdown(nameof(GetLangs))] private string currentLanguage = "en";
        private string[] GetLangs => GlobalsUtils.GetNames(typeof(LanguageKeys));

        void OnEnable()
        {
            LoadLanguage();
        }

        [Button("Load Language")]
        public void LoadLanguage()
        {
            LocalizationManager.LoadLanguage(currentLanguage);
        }
    }
}