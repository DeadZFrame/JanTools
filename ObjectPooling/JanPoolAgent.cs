using UnityEngine;

namespace Jan.Pool
{
    public class JanPoolAgent : MonoBehaviour
    {
        void OnDisable()
        {
            this.Dispose();
        }
    }
}