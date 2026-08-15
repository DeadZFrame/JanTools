using Jan.UI;
using UnityEngine;
using UnityEngine.UI;
using Jan.Feel;
using Jan.Tasks;
using JeffGrawAssets.FlexibleUI;

namespace UI
{
    public class WarningContainer : TextContainer, IWarningUI
    {
        [SerializeField] private Image interactionImage;
        [SerializeField] private Image dividerImage;
        [SerializeField] private Sprite warningIcon, infoIcon;
        [SerializeField] private GFeedback warningFeedback;
        [SerializeField] private BlurredImage blurredImage;
        [SerializeField] private Color warningColor, infoColor;
        
        private Cts _cts;

        public void SetWarningText(string text, bool isWarning)
        {
            base.SetText(text);

            var icon = GetIcon(isWarning);
            if (icon != null)
            {
                interactionImage.gameObject.SetActive(true);
                dividerImage.gameObject.SetActive(false);
                interactionImage.sprite = icon;
            }
            else
            {
                dividerImage.gameObject.SetActive(false);
                interactionImage.gameObject.SetActive(false);
            }

            if (isWarning)
            {
                warningFeedback.Play();
            }

            _cts?.SafeCancel();
            _cts = Timed.CallDelayed(3f, () => Show(false));
        }

        private Sprite GetIcon(bool isWarning)
        {
            blurredImage.color = isWarning ? warningColor : infoColor;
            return isWarning ? warningIcon : infoIcon;
        }
    }
}