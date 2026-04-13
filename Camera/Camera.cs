using UnityEngine;

namespace Jan.Core
{
    public class Camera : MonoBehaviour
    {
        [field: SerializeField] public UnityEngine.Camera CameraComponent { get; private set; }
        [field: SerializeField] public AudioListener AudioListener { get; private set; }

        void Awake()
        {
            CameraManager.Instance.RegisterCamera(this);
        }
    }
}