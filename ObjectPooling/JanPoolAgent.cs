using Jan.Events;
using UnityEngine;

namespace Jan.Pool
{
    public class JanPoolAgent : MonoBehaviour
    {
        void OnDisable()
        {
            this.Dispose();
            EventManager.Dispose();
        }
    }
}