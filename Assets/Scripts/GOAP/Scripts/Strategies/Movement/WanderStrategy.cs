using UnityEngine;
using UnityEngine.AI;
using Utilities;

namespace GOAP.Scripts
{
    public class WanderStrategy : IActionStrategy
    {
        private readonly NavMeshAgent agent;
        private readonly float wanderRadius;

        public WanderStrategy(NavMeshAgent agent, float wanderRadius)
        {
            this.agent = agent;
            this.wanderRadius = wanderRadius;
        }

        public bool CanPerform => !Complete;
        public bool Complete => agent.remainingDistance <= 2f && !agent.pathPending;

        public void Start()
        {
            for (var i = 0; i < 5; i++)
            {
                var randomDirection = (Random.insideUnitSphere * wanderRadius).With(y: 0);
                NavMeshHit hit;

                if (NavMesh.SamplePosition(agent.transform.position + randomDirection, out hit, wanderRadius, 1))
                {
                    agent.SetDestination(hit.position);
                    return;
                }
            }
        }
    }
}