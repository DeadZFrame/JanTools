using System;
using Jan.Core;
using Sirenix.OdinInspector;
using Jan.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Jan.Interaction
{
    public class InteractionUI : TextContainer, IInteractionUI
    {
        [Serializable]
        public class InteractionIcons
        {
            [field: SerializeField, ValueDropdown(nameof(GetInteractionIconNames))] public string Name { get; private set; }
            [field: SerializeField] public Sprite Icon { get; private set; }

            public string[] GetInteractionIconNames => Jan.Core.GlobalsUtils.GetNames(typeof(InteractionIconNames));
        }

        [SerializeField] private InteractionIcons[] interactionIcons;

        [SerializeField] private Image interactionImage;
        [SerializeField] private Image dividerImage;

        public void SetTextAndIcon(string text, string iconName)
        {
            base.SetText(text);

            var icon = GetIcon(iconName);
            if (icon != null)
            {
                dividerImage.gameObject.SetActive(true);
                interactionImage.sprite = icon;
            }
            else
            {
                dividerImage.gameObject.SetActive(false);
            }
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