using UnityEngine;
using UnityEngine.UI;
using Jan.Core;
using Sirenix.OdinInspector;
using TMPro;

namespace Jan.UI
{
    public class BarContainer : UIElement, IBarContainer, IMotion
    {
        Jan.Core.Motion IMotion.MotionHandle { get; set; }

        [SerializeField] private Image bar;
        [SerializeField] private TextMeshProUGUI textLabel;
        [SerializeField] private Image handle;
        [SerializeField, ShowIf(nameof(animateColor))] private Gradient gradient;
        [SerializeField] private bool animateColor;

        public virtual void SetFillAmount(float amount, float duration, Ease ease = Ease.Linear)
        {
            this.FloatMotion(bar.fillAmount, amount, duration, ease);
        }

        public void SetHandle(bool active)
        {
            if(handle != null) handle.gameObject.SetActive(active);
        }

        public void SetHandlePosition(float amount)
        {
            if (handle == null) return;
            amount *= 135;
            handle.rectTransform.anchoredPosition = new Vector2(amount, handle.rectTransform.anchoredPosition.y);
        }

        public void SetColor(Color color)
        {
            if(bar != null)
            {
                bar.color = color;
            }
        }
        
        void IMotion.SetFloat(float value)
        {
            bar.fillAmount = value;
            
            if(textLabel != null)
            {
                textLabel.SetText($"{Mathf.RoundToInt(value * 100)}%");
            }
            
            if (animateColor)
            {
                bar.color = gradient.Evaluate(value);
            }
        }
    }
}