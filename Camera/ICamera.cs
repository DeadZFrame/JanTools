using UnityEngine;

namespace Jan.Core
{
    public interface ICamera
    {
        CameraBase Camera { get; }
        AudioListener AudioListener { get; }
    }
}

