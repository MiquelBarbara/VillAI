using System.Collections.Generic;
using UnityEngine;

namespace Game.ALPHA
{
    /// <summary>
    /// Selects the closest detectable object to the agent.
    /// </summary>
    public class ProximityTargetSelectionCommand : ITargetSelectionCommand
    {
        public T Execute<T>(List<T> detectables, Vector3 agentPosition) where T : Component
        {
            T closest = null;
            float closestDistance = float.MaxValue;

            foreach (var detectable in detectables)
            {
                // Calculate the distance from the agent
                float distance = Vector3.Distance(agentPosition, detectable.transform.position);
                // If the distance is effectively zero, this is likely the agent itself, so skip it.
                if (distance < 0.01f) continue;

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = detectable;
                }
            }

            return closest;
        }
    }
}