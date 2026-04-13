using UnityEngine;

namespace Jan.Core
{
    public interface ICamera
    {
        Camera Camera { get; }
        AudioListener AudioListener { get; }
    }
}

