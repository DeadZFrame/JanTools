using Jan.Core;
using UnityEngine;

namespace Jan.UI
{
    public interface IBarContainer : IUIElement
    {
        void SetFillAmount(float amount, float duration, Ease ease = Ease.Linear);
        void SetColor(Color color);
    }
}