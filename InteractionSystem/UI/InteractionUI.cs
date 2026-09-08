using System;
using Jan.Core;
using Sirenix.OdinInspector;
using Jan.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Jan.Interaction
{
    public class InteractionUI : UIElement, IInteractionUI
    {
        [Serializable]
        public class InteractionIcons
        {
            [field: SerializeField, ValueDropdown(nameof(GetInteractionIconNames))] public string Name { get; private set; }
            [field: SerializeField] public Sprite Icon { get; private set; }

            public string[] GetInteractionIconNames => GlobalsUtils.GetNames(typeof(InteractionIconNames));
        }

        [SerializeField] private TextMeshProUGUI leftClickText, rightClickText;
        [SerializeField] private InteractionIcons[] interactionIcons;
        [SerializeField] private Image circleFill;
        [SerializeField] private Image interactionImage;
        [SerializeField] private Image dividerImage;

        public void SetTextAndIcon(string text, string iconName)
        {
            var splitText = text.Split('|');
            string rightClickTooltip;
            string leftClickTooltip;

            if(splitText.Length > 1)
            {
                rightClickTooltip = splitText[1];
                leftClickTooltip = splitText[0];
            }
            else
            {
                rightClickTooltip = "";
                leftClickTooltip = splitText[0];
            }

            var icon = GetIcon(iconName);
            if (icon != null)
            {
                interactionImage.gameObject.SetActive(true);
                dividerImage.gameObject.SetActive(true);
                interactionImage.sprite = icon;
            }
            else
            {
                dividerImage.gameObject.SetActive(false);
                interactionImage.gameObject.SetActive(false);
            }

            if(string.IsNullOrEmpty(rightClickTooltip))
            {
                rightClickText.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                rightClickText.transform.parent.gameObject.SetActive(true);
                rightClickText.SetText(rightClickTooltip);
            }

            if(string.IsNullOrEmpty(leftClickTooltip))
            {
                leftClickText.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                leftClickText.transform.parent.gameObject.SetActive(true);
                leftClickText.SetText(leftClickTooltip);
            }
        }

        public void SetInteractionProgress(float progress)
        {
            circleFill.fillAmount = progress;
        }

        private Sprite GetIcon(string iconName)
        {
            for (int i = 0; i < interactionIcons.Length; i++)
            {
                if (interactionIcons[i].Name == iconName)
                {
                    return interactionIcons[i].Icon;
                }
            }

            return null;
        }
    }
}