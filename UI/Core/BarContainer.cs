using UnityEngine;
using UnityEngine.UI;
using Jan.Core;

namespace Jan.UI
{
    public class BarContainer : UIElement, IBarContainer, IMotion
    {
        Jan.Core.Motion IMotion.MotionHandle { get; set; }

        [SerializeField] private Image _bar;

        public void SetFillAmount(float amount)
        {
            this.FloatMotion(_bar.fillAmount, amount, .25f, Jan.Core.Ease.OutSine);
        }

        public void SetFloat(float value)
        {
            _bar.fillAmount = value;
        }
    }
}