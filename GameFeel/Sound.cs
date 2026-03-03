using System;
using Jan.Core;
using Jan.Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Giant.Feel
{
    [Serializable]
    public class Sound : FeedbackBase
    {
        [ValueDropdown(nameof(GetSounds)), SerializeField]
        private string sound = "";
        private string[] GetSounds => GlobalsUtils.GetNames(typeof(SoundNames));

        public override FeedbackBase Play(Transform transform)
        {
            if (string.IsNullOrEmpty(sound))
                return this;

            bool soundPlayed = false;

            // Log warning if neither exists
            if (!soundPlayed)
            {
                Debug.LogWarning($"Sound '{sound}' not found as either Sound Group or Custom Event in MasterAudio");
            }

            return this;
        }

        public override void Complete()
        {
            // Sound feedback has no ongoing sequences or tweens to complete
            // Audio clips play instantly and don't need completion handling
        }
    }
}