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
            Debug.LogWarning("Sound feedback does not support Complete() - it will play to completion once triggered. Use Stop() if you need to trigger any stop logic immediately after playing.");
        }

        public override void Stop()
        {
            Debug.LogWarning("Sound feedback does not support Stop() - it will play to completion once triggered. Use Complete() if you need to trigger any completion logic immediately after playing.");
        }
    }
}