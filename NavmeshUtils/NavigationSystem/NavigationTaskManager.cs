using System.Collections.Generic;
using Jan.Core;
using Jan.Tasks;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace Jan.Navigation
{
    public partial class NavigationTaskManager : Singleton<NavigationTaskManager>
    {
        private readonly List<IAgent> registeredAgents = new List<IAgent>();
        public IReadOnlyList<IAgent> Agents => registeredAgents;
        private int _currentBatchIndex = 0;
        private const int BATCH_SIZE = 128;
        [SerializeField] private float destinationCheckPeriod = .1f; 
        [field: SerializeField] public NavMeshSurface NavMeshSurface { get; private set; }

        // Pre-computed random directions for optimization (8 cardinal + diagonal directions)
        private static readonly Vector3[] _precomputedDirections = new Vector3[]
        {
            new Vector3(1f, 0f, 0f),      // East
            new Vector3(-1f, 0f, 0f),     // West  
            new Vector3(0f, 0f, 1f),      // North
            new Vector3(0f, 0f, -1f),     // South
            new Vector3(0.707f, 0f, 0.707f),   // Northeast
            new Vector3(-0.707f, 0f, 0.707f),  // Northwest
            new Vector3(0.707f, 0f, -0.707f),  // Southeast
            new Vector3(-0.707f, 0f, -0.707f)  // Southwest
        };

        // Fast random state for better performance than UnityEngine.Random
        private static readonly uint _randomState = (uint)System.Environment.TickCount;
        
        void Start()
        {
            Timed.CallPeriodically(double.MaxValue, destinationCheckPeriod, ManageNavigation, gameObject);
        }

        private void ManageNavigation()
        {
            int agentCount = this.registeredAgents.Count;
            if (agentCount == 0) return;

            int startIndex = _currentBatchIndex;
            int endIndex = Mathf.Min(startIndex + BATCH_SIZE, agentCount);
            var registeredAgents = Agents;

            for (int i = startIndex; i < endIndex; i++)
            {
                if (!registeredAgents.IsIndexValid(i, out var agent)) continue; // Skip invalid indices
                ProcessAgent(agent);
            }

            _currentBatchIndex = endIndex >= agentCount ? 0 : endIndex;
        }

        private void ProcessAgent(IAgent agent)
        {
            var navAgent = agent.Agent;

            if (navAgent == null || !navAgent.isActiveAndEnabled || !navAgent.enabled || !navAgent.isOnNavMesh)
            {
                return;
            }

            if (navAgent.pathPending || agent.IsDestinationReached) return;

            if (HasReachedDestination(agent, navAgent))
            {
                navAgent.ResetPath();
                agent.OnJobDestinationReached();
                return;
            }
        }

        private bool HasReachedDestination(IAgent agent, NavMeshAgent navAgent)
        {
            var destination = navAgent.destination;

            if (navAgent.isPathStale)
            {
                var areaMask = (NavmeshAreas)navAgent.areaMask;
                var newDest = NavMeshUtils.GetRandomPointOnCircle(destination, 5, areaMask);
                navAgent.SetDestination(newDest);
                return false;
            }

            if (navAgent == null || !navAgent.isActiveAndEnabled || !navAgent.enabled || !navAgent.isOnNavMesh)
            {
                return true;
            }

            var hasReachedDestination = navAgent.remainingDistance <= agent.DestinationReachedThreshold;
            return hasReachedDestination;
        }

        public void RegisterAgent(IAgent agent)
        {
            if (!registeredAgents.Contains(agent)) // Prevent duplicates
            {
                registeredAgents.Add(agent);
            }
        }

        public void UnregisterAgent(IAgent agent)
        {
            registeredAgents.Remove(agent);
        }

        // Helper method to avoid Vector3 allocations for random directions
        // Uses pre-computed directions and fast random for better performance
        private static void GetRandomDirection(ref Vector3 direction)
        {
            var randomState = _randomState;
            // Fast XorShift random number generator (much faster than UnityEngine.Random)
            randomState ^= randomState << 13;
            randomState ^= randomState >> 17;
            randomState ^= randomState << 5;

            var precomputedDirections = _precomputedDirections;

            // Use pre-computed directions instead of calculating random values
            int index = (int)(randomState % (uint)precomputedDirections.Length);
            direction = precomputedDirections[index];
        }
    }
}