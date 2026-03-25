using System.Collections.Generic;
using Jan.Core;
using UnityEngine;
using UnityEngine.Pool;

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