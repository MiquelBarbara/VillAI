using System;
using UnityEngine;
using UnityEngine.AI;

namespace GOAP.Scripts
{
    public class MoveToStrategy: IActionStrategy
    {
        private GameObject agent;
        private AgentBelief agentBelief;
        private NavMeshAgent Agent => agent.GetComponent<NavMeshAgent>();
        
        public MoveToStrategy(GameObject agent, AgentBelief agentBelief)
        {
            this.agent = agent;
            this.agentBelief = agentBelief;
        }
        
        public void Start()
        {
            Agent.isStopped = false;
            Agent.SetDestination(agentBelief.Location);
        }
        
        public void Update(float deltaTime)
        {
            
        }
        
        public void Stop()
        {
            Agent.ResetPath();
            Agent.isStopped = true;
            Agent.velocity = Vector3.zero;
        }
        
        public bool CanPerform => !Complete;
        public bool Complete => Agent.hasPath && !Agent.pathPending && Agent.remainingDistance <= 0.4f;
    }
}