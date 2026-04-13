using System;
using Jan.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Jan.Feel
{
    public abstract class FeedbackBase
    {
        [field: SerializeField, ValidateInput(nameof(IsPositiveDuration), "Value must be positive")]
        public float Duration { get; set; } = 1;

        [field: SerializeField, ValidateInput(nameof(IsPositiveDelay), "Value must be positive or zero")]
        public float Delay { get; set; } = 0;

        private bool IsPositiveDuration => Duration >= 0;
        private bool IsPositiveDelay => Delay >= 0;

        private bool IsAppPlaying => Application.isPlaying;
        [Button, ShowIf(nameof(IsAppPlaying)), GUIColor(0.75f, .5f, .5f)]
        public abstract FeedbackBase Play(Transform transform);
        public abstract void Complete();
        public abstract void Stop();
        public virtual void OnComplete(Action callback, GameObject cullingObject = null)
        {
            var time = Delay + Duration;
            Timed.CallDelayed(time, callback, cullingObject);
        }
    }
}
