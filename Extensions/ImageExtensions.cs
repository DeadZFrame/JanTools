using Jan.Tasks;
using UnityEngine;

namespace Jan.Core
{
    public static class ImageExtensions
    {
        public static Cts AnimateFillAmount(this UnityEngine.UI.Image image, float target, float duration, Ease ease = Ease.Linear)
        {
            if (image == null) return null;

            var initialValue = image.fillAmount;
            var elapsedTime = 0f;

            return Timed.CallContinuesFor(duration, () =>
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                t = ease.Evaluate(t);
                image.fillAmount = Mathf.Lerp(initialValue, target, t);
            }).OnCompleted(() => image.fillAmount = target);
        }
    }
}