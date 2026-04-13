using Jan.Tasks;
using TMPro;
using UnityEngine;

namespace Jan.Core
{
    public static class TextExtensions
    {
        public static Cts AnimateValue(this TextMeshProUGUI textComponent, int from, int to, float duration, string format = "F1", string suffix = "", string prefix = "")
        {
            if (textComponent == null) return null;

            return Timed.CallContinuesFor(duration, () =>
            {
                float t = Mathf.Clamp01(Time.time / duration);
                int value = Mathf.RoundToInt(Mathf.Lerp(from, to, t));
                textComponent.text = prefix + value.ToString(format) + suffix;
            }).OnCompleted(() => textComponent.text = prefix + to.ToString(format) + suffix);
        }
    }
}