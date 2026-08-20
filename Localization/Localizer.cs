using Jan.Core;
using Jan.Events;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Jan.Localization
{
    public class Localizer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textElement;
        [SerializeField, ValueDropdown(nameof(GetLocalizationContexts))] private string localizationContext;
        [SerializeField, ValueDropdown(nameof(GetDialogueIds))] private string localizationKey;

        private string[] GetLocalizationContexts => LocalizationManager.GetContextNames();
        private string[] GetDialogueIds => LocalizationManager.GetContext(localizationContext);

        void OnEnable()
        {
            EventManager.Register(EventNames.OnLanguageLoaded, UpdateText);
        }

        void OnDisable()
        {
            EventManager.UnRegister(EventNames.OnLanguageLoaded, UpdateText);
        }

        private void UpdateText()
        {
            if (textElement != null)
            {
                textElement.SetText(LocalizationManager.GetLocalizedValue(localizationContext, localizationKey));
            }
        }
    }
}