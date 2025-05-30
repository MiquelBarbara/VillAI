using Blackboard_Architecture;
using UnityEngine;
using UnityEngine.AI;

namespace GOAP.Scripts
{
    public class MoveStrategy : IActionStrategy
    {
        private readonly GameObject agent;
        private readonly Blackboard blackboard;
        private readonly BlackboardKey blackboardKey;

        public MoveStrategy(GameObject agent, Blackboard blackboard, BlackboardKey key)
        {
            this.agent = agent;
            this.blackboard = blackboard;
            this.blackboardKey = key;
        }

        private NavMeshAgent Agent => agent.GetComponent<NavMeshAgent>();
        public bool CanPerform => !Complete;
        public bool Complete => Agent.remainingDistance <= 1f && !Agent.pathPending;

        public void Start()
        {
            if (blackboard.TryGetValue(blackboardKey, out Vector3 target))
            {
                Agent.SetDestination(target);
            }
            
            if(blackboard.TryGetValue(blackboardKey, out Transform targetTransform))
            {
                Agent.SetDestination(targetTransform.position);
            }
        }
        public void Stop()
        {
            Agent.ResetPath();
        }
    }
}