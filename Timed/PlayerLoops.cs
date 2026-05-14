using Cysharp.Threading.Tasks;

namespace Jan.Tasks
{
    public enum PlayerLoops
    {
        // Include all original PlayerLoopTiming values
        Initialization = PlayerLoopTiming.Initialization,
        LastInitialization = PlayerLoopTiming.LastInitialization,

        EarlyUpdate = PlayerLoopTiming.EarlyUpdate,
        LastEarlyUpdate = PlayerLoopTiming.LastEarlyUpdate,

        FixedUpdate = PlayerLoopTiming.FixedUpdate,
        LastFixedUpdate = PlayerLoopTiming.LastFixedUpdate,

        PreUpdate = PlayerLoopTiming.PreUpdate,
        LastPreUpdate = PlayerLoopTiming.LastPreUpdate,

        Update = PlayerLoopTiming.Update,
        LastUpdate = PlayerLoopTiming.LastUpdate,

        PreLateUpdate = PlayerLoopTiming.PreLateUpdate,
        LastPreLateUpdate = PlayerLoopTiming.LastPreLateUpdate,

        PostLateUpdate = PlayerLoopTiming.PostLateUpdate,
        LastPostLateUpdate = PlayerLoopTiming.LastPostLateUpdate,

        #if UNITY_2020_2_OR_NEWER
        
        TimeUpdate = PlayerLoopTiming.TimeUpdate,
        LastTimeUpdate = PlayerLoopTiming.LastTimeUpdate,

        #endif
    }
}
  
