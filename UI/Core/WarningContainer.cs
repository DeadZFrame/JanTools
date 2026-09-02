using Jan.UI;
using UnityEngine;
using UnityEngine.UI;
using Jan.Feel;
using Jan.Tasks;
using JeffGrawAssets.FlexibleUI;
using Jan.Core;
using TMPro;

namespace UI
{
    public class WarningContainer : UIElement, IWarningUI
    {
        [SerializeField] private TextMeshProUGUI textMesh;
        [SerializeField] private Image interactionImage;
        [SerializeField] private Image dividerImage;
        [SerializeField] private Sprite warningIcon, infoIcon;
        [SerializeField] private GFeedback warningFeedback, notificationFeedback;
        [SerializeField] private BlurredImage blurredImage;
        [SerializeField] private Color warningColor, infoColor;
        
        private Cts _cts;

        public void SetWarningText(string text, bool isWarning)
        {
            textMesh.SetText(text);
            Show(true);

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
                warningFeedback.Complete();
                warningFeedback.Play();
                SoundLibrary.PlaySound(SoundNames.NegativeWarning);
            }
            else
            {
                notificationFeedback.Complete();
                notificationFeedback.Play();
                SoundLibrary.PlaySound(SoundNames.PositiveWarning);
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