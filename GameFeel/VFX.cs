using System;
using UnityEngine;

namespace Giant.Feel
{
    [Serializable]
    public class VFX : FeedbackBase
    {
        [field: SerializeField] public VFXAgent VFXAgent {get; private set;}
        [field: SerializeField] public bool parented { get; private set; }
        
        public override FeedbackBase Play(Transform transform)
        {
            return this;
        }

        public override void Complete()
        {
            VFXAgent.Stop();
        }

        public override void Stop()
        {
             Debug.LogWarning("VFX feedback does not support Stop() - it will complete naturally based on its duration. Use Complete() to immediately stop the effect.");
        }
    }
}
