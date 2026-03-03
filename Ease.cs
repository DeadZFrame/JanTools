using UnityEngine;

namespace Jan.Core
{
    public enum Ease
    {
        Linear,
        EaseIn,
        EaseOut,
        EaseInOut,
        InSine,
        OutSine,
        InOutSine,
        InQuad,
        OutQuad,
        InOutQuad,
        InCubic,
        OutCubic,
        InOutCubic,
        InQuart,
        OutQuart,
        InOutQuart,
        InQuint,
        OutQuint,
        InOutQuint,
        InExpo,
        OutExpo,
        InOutExpo,
        InCirc, 
        OutCirc,
        InOutCirc,
        InElastic,
        OutElastic,
        InOutElastic,
        InBack,
        OutBack,
        InOutBack,
        InBounce,
        OutBounce,
        InOutBounce
    }

    public static class EaseExtensions
    {
        public static float Evaluate(this Ease ease, float t)
        {
            return ease switch
            {
                Ease.EaseIn => t * t,
                Ease.EaseOut => t * (2 - t),
                Ease.EaseInOut => t < 0.5f ? 2 * t * t : -1 + (4 - 2 * t) * t,
                Ease.InSine => 1 - Mathf.Cos((t * Mathf.PI) / 2),
                Ease.OutSine => Mathf.Sin((t * Mathf.PI) / 2),
                Ease.InOutSine => -(Mathf.Cos(Mathf.PI * t) - 1) / 2,
                Ease.InQuad => t * t,
                Ease.OutQuad => t * (2 - t),
                Ease.InOutQuad => t < 0.5f ? 2 * t * t : -1 + (4 - 2 * t) * t,
                Ease.InCubic => t * t * t,
                Ease.OutCubic => 1 - Mathf.Pow(1 - t, 3),
                Ease.InOutCubic => t < 0.5f ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2,
                Ease.InQuart => t * t * t * t,
                Ease.OutQuart => 1 - Mathf.Pow(1 - t, 4),
                Ease.InOutQuart => t < 0.5f ? 8 * t * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 4) / 2,
                Ease.InQuint => t * t * t * t * t,
                Ease.OutQuint => 1 - Mathf.Pow(1 - t, 5),
                Ease.InOutQuint => t < 0.5f ? 16 * t * t * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 5) / 2,
                Ease.InExpo => t == 0 ? 0 : Mathf.Pow(2, 10 * (t - 1)),
                Ease.OutExpo => t == 1 ? 1 : 1 - Mathf.Pow(2, -10 * t),
                Ease.InOutExpo => t == 0 ? 0 : t == 1 ? 1 : (t < 0.5f ? Mathf.Pow(2, 20 * t - 10) / 2 : (2 - Mathf.Pow(2, -20 * t + 10)) / 2),
                Ease.InCirc => 1 - Mathf.Sqrt(1 - Mathf.Pow(t, 2)),
                Ease.OutCirc => Mathf.Sqrt(1 - Mathf.Pow(t - 1, 2)),
                Ease.InOutCirc => t < 0.5f ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * t, 2))) / 2 : (Mathf.Sqrt(1 - Mathf.Pow(-2 * t + 2, 2)) + 1) / 2,
                Ease.InElastic => t == 0 ? 0 : t == 1 ? 1 : -Mathf.Pow(2, 10 * t - 10) * Mathf.Sin((t * 10 - 10.75f) * ((2 * Mathf.PI) / 3)),
                Ease.OutElastic => t == 0 ? 0 : t == 1 ? 1 : Mathf.Pow(2, -10 * t) * Mathf.Sin((t * 10 - 0.75f) * ((2 * Mathf.PI) / 3)) + 1,
                Ease.InOutElastic => t == 0 ? 0 : t == 1 ? 1 : (t < 0.5f ? -(Mathf.Pow(2, 20 * t - 10) * Mathf.Sin((20 * t - 11.125f) * ((2 * Mathf.PI) / 4.5f))) / 2 : (Mathf.Pow(2, -20 * t + 10) * Mathf.Sin((20 * t - 11.125f) * ((2 * Mathf.PI) / 4.5f)) / 2 + 1)),
                Ease.InBack => 2.70158f * t * t * t - 1.70158f * t * t,
                Ease.OutBack => 1 + 2.70158f * Mathf.Pow(t - 1, 3) + 1.70158f * Mathf.Pow(t - 1, 2),
                Ease.InOutBack => t < 0.5f ? (Mathf.Pow(2 * t, 2) * ((2.70158f * 1.525f + 1) * 2 * t - 2.70158f * 1.525f)) / 2 : (Mathf.Pow(2 * t - 2, 2) * ((2.70158f * 1.525f + 1) * (t * 2 - 2) + 2.70158f * 1.525f) + 2) / 2,
                Ease.InBounce => 1 - Evaluate(Ease.OutBounce, 1 - t),
                Ease.OutBounce => t < 1 / 2.75f ? 7.5625f * t * t : t < 2 / 2.75f ? 7.5625f * (t -= 1.5f / 2.75f) * t + 0.75f : t < 2.5 / 2.75f ? 7.5625f * (t -= 2.25f / 2.75f) * t + 0.9375f : 7.5625f * (t -= 2.625f / 2.75f) * t + 0.984375f,
                _ => t, // Linear
            };
        }
    }
}