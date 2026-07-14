using UnityEngine;
using UnityEngine.UI;
using Jan.Core;
using Sirenix.OdinInspector;

namespace Jan.UI
{
    public class BarContainer : UIElement, IBarContainer, IMotion
    {
        Jan.Core.Motion IMotion.MotionHandle { get; set; }

        [SerializeField] private Image bar;
        [SerializeField] private Image handle;
        [SerializeField, ShowIf(nameof(animateColor))] private Gradient gradient;
        [SerializeField] private bool animateColor;

        public void SetFillAmount(float amount, float duration, Ease ease = Ease.Linear)
        {
            this.FloatMotion(bar.fillAmount, amount, duration, ease);
            if (handle == null) return;
            this.FloatMotion(handle.rectTransform.anchoredPosition.x, amount * bar.rectTransform.sizeDelta.x, duration, ease);
        }

        public void SetHandle(bool active)
        {
            handle.gameObject.SetActive(active);
        }
        
        void IMotion.SetFloat(float value)
        {
            bar.fillAmount = value;
            
            if (animateColor)
            {
                bar.color = gradient.Evaluate(value);
            }
        }
    }
}