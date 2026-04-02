using System.Collections.Generic;
using Jan.Maths;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Jan.Navigation
{
    /// <summary>
    /// Provides utility methods for NavMesh-based position calculations and path finding.
    /// </summary>
    public static class NavMeshUtils
    {
        private static NavMeshHit hit;
        public const float AgentRadius = .8f;

        /// <summary>
        /// Gets a random point on the perimeter of a circle on the NavMesh.
        /// </summary>
        /// <param name="sourcePosition">The center point of the circle</param>
        /// <param name="range">The radius of the circle</param>
        /// <param name="areaMask">The NavMesh area mask to sample points from</param>
        /// <returns>A valid NavMesh position on the circle's perimeter, or Vector3.zero if no valid position is found</returns>
        /// /// <remarks>
        /// This method will attempt to find a valid NavMesh position on the circle's perimeter.
        /// If no valid position is found within 1000 units, returns Vector3.zero.
        /// </remarks>
        public static Vector3 GetRandomPointOnCircle(Vector3 sourcePosition, float range, NavmeshAreas areaMask)
        {
            var randomPosition = sourcePosition + VectorExtensions.GetRandomPointOnUnitCircle().ConvertToVector3() * range;
            var isHit = NavMesh.SamplePosition(randomPosition, out hit, 100f, (int)areaMask);

            return isHit ? hit.position : Vector3.zero;
        }

        /// <summary>
        /// Gets a random point inside a circle on the NavMesh.
        /// </summary>
        /// <param name="sourcePosition">The center point of the circle</param>
        /// <param name="range">The radius of the circle</param>
        /// <param name="areaMask">The NavMesh area mask to sample points from</param>
        /// <param name="forceInside">When true, performs up to 10 retry attempts with stricter NavMesh sampling (2 unit radius) to ensure the point is found within the specified circle. When false, uses a single attempt with relaxed sampling (1000 unit radius) that may find points outside the original circle bounds.</param>
        /// <returns>A valid NavMesh position inside the circle, or Vector3.zero if no valid position is found</returns>
        /// <remarks>
        /// Uses Random.insideUnitCircle to generate points within the circle area.
        /// The forceInside parameter controls the trade-off between accuracy and performance:
        /// - forceInside = true: More accurate positioning within the circle but may fail more often
        /// - forceInside = false: Faster execution but may return points outside the intended area
        /// Special handling for Water areas: validates that the position is exclusively on water NavMesh.
        /// </remarks>
        public static Vector3 GetRandomPointInCircle(Vector3 sourcePosition, float range, NavmeshAreas areaMask, bool forceInside = false)
        {
            bool isHit = false;

            var maxTrials = 10;
            if (!forceInside) maxTrials = 1;

            while (!isHit)
            {
                var randomPosition = sourcePosition + Random.insideUnitCircle.ConvertToVector3() * range;
                isHit = NavMesh.SamplePosition(randomPosition, out hit, forceInside ? 2f : 1000f, (int)areaMask);

                if (isHit && areaMask == NavmeshAreas.Water)
                {
                    int otherAreasMask = ~(int)areaMask;
                    var anotherLayer = NavMesh.SamplePosition(randomPosition, out var _, 2f, otherAreasMask);
                    if (anotherLayer) isHit = false;
                }

                maxTrials--;
                if (maxTrials == 0) break;
            }

            return isHit ? hit.position : Vector3.zero;
        }

        /// <summary>
        /// Gets a random point on a conic section of the NavMesh.
        /// </summary>
        /// <param name="sourcePosition">The apex/starting point of the cone</param>
        /// <param name="direction">The central direction of the cone</param>
        /// <param name="angle">The total angle of the cone in degrees</param>
        /// <param name="range">The distance from the apex to sample points</param>
        /// <param name="areaMask">The NavMesh area mask to sample points from</param>
        /// <returns>A valid NavMesh position within the cone, or Vector3.zero if no valid position is found</returns>
        /// <remarks>
        /// The angle parameter defines the total cone angle. For example, an angle of 90 degrees
        /// will sample points within ±45 degrees of the direction vector.
        /// Returns Vector3.zero if no valid NavMesh position is found within 1000 units.
        /// </remarks>
        public static Vector3 GetRandomPointOnConic(Vector3 sourcePosition, Vector3 direction, float angle, float range, NavmeshAreas areaMask)
        {
            direction.Normalize();

            float randomAngle = Random.Range(-angle / 2f, angle / 2f);

            Quaternion rotation = Quaternion.AngleAxis(randomAngle, Vector3.up);
            Vector3 rotatedDirection = rotation * direction;

            var randomPosition = sourcePosition + rotatedDirection * range;
            var isHit = NavMesh.SamplePosition(randomPosition, out hit, 100f, (int)areaMask);

            return isHit ? hit.position : Vector3.zero;
        }

        public static void SetDestination(this IAgent agent, Vector3 destination, params IReadOnlyList<IAgent>[] otherAgents)
        {
            Vector3 displacement = Vector3.zero;

            if (otherAgents == null || otherAgents.Length == 0)
            {
                agent.Agent.SetDestination(destination);
                return;
            }

            const float AgentRadiusSqr = AgentRadius * AgentRadius;

            for (int i = 0; i < otherAgents.Length; i++)
            {
                try
                {
                    var item = otherAgents[i];
                    Vector3 direction = destination - item[0].AgentTransform.position;
                    float distance = direction.sqrMagnitude;

                    // If inside the zone, calculate displacement needed
                    if (distance < AgentRadiusSqr)
                    {
                        if (distance < .001f)
                        {
                            // If exactly at NPC position, use random direction
                            direction = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f)).normalized;
                        }
                        else
                        {
                            direction = direction.normalized;
                        }

                        // Calculate how far to push out
                        float pushDistance = AgentRadiusSqr - distance;
                        displacement += direction * pushDistance;
                    }
                }
                catch
                {
                    Debug.LogWarning($"Error processing agent at index {i} in SetDestination. Ensure the agent is valid and has a NavMeshAgent component.");
                }
            }

            agent.Agent.SetDestination(destination + displacement);
        }

        public static T GetNearest<T>(T[] array, Vector3 currentPos, NavmeshAreas areaMask) where T : MonoBehaviour
        {
            float minDistance = float.MaxValue;
            T nearest = null;
            
            for (int i = 0; i < array.Length; i++)
            {
                var element = array[i];

                float dist = 0;
                NavMeshPath path = new NavMeshPath();
                
                if (NavMesh.CalculatePath(currentPos, element.transform.position, (int)areaMask, path))
                {
                    for (int l = 1; l < path.corners.Length; l++)
                    {
                        dist += (path.corners[l - 1] - path.corners[l]).sqrMagnitude;
                    }
                }
  
                if (dist < minDistance)
                {
                    minDistance = dist;
                    nearest = element;
                }
            }

            return nearest;
        }
    }
}