using UnityEngine;
using UnityEngine.AI;

namespace Jan.Navigation
{
    public interface IAgent
    {
        NavMeshAgent Agent { get; }
        float DestinationReachedThreshold { get; }
        bool IsDestinationReached { get; }
        Vector3 Destination { get; }
        Transform AgentTransform { get; }
        
        void OnJobDestinationReached();        
    }
}