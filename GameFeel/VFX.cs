using System;
using Jan.Pool;
using UnityEngine;

namespace Jan.Feel
{
    [Serializable]
    public class VFX : FeedbackBase
    {
        [field: SerializeField] public VFXAgent VFXAgent {get; private set;}
        [field: SerializeField] public bool parented { get; private set; }
        
        public override FeedbackBase Play(Transform transform)
        {
            var vfx = JanPool.Spawn(VFXAgent, transform.position, transform.rotation);
            if(parented)
            {
                vfx.transform.SetParent(transform);
            }

            vfx.Play();

            Debug.Log("VFX played.");
            return this;
        }

        public override void Complete()
        {
            Debug.LogWarning("VFX feedback does not support Complete() - it will complete naturally based on its duration.");
        }

        public override void Stop()
        {
            Debug.LogWarning("VFX feedback does not support Stop() - it will complete naturally based on its duration. Use Complete() to immediately stop the effect.");
        }
    }
}
