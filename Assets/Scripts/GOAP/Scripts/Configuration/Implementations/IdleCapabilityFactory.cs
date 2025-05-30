using GOAP.Scripts;
using GOAP.Scripts.Configuration.Capabilities;
using UnityEngine.AI;

namespace Game.GOAP.Scripts.Configuration.Implementations
{
    /// <summary>
    /// Represents the configuration for the Idle capability in a GOAP agent.
    /// </summary>
    public class IdleCapabilityConfig : ICapabilityConfig
    {
        public void Configure(GoapAgent agent)
        {
            /// Create a new CapabilityBuilder for the Idle capability.
            var builder = new CapabilityBuilder("IdleCapability");
            
            /// ------------------------BELIEFS------------------------///
            // Belief related to the agent's movement state, in this case moving state.
            var beliefAgentMoving = new AgentBelief.Builder("AgentMoving").WithCondition(() =>
                agent.GetComponent<NavMeshAgent>().velocity.magnitude > 0.01f).Build();
            // Belief related to the agent's movement state, in this case idle state.
            var beliefAgentIdle = new AgentBelief.Builder("AgentIdle").WithCondition(() =>
                !(agent.GetComponent<NavMeshAgent>().velocity.magnitude > 0.01f)).Build();

            //Empty belief that is always false so the agent does always something.
            var beliefNothing = new AgentBelief.Builder("Nothing").WithCondition(() => false).Build();
            
            // Add beliefs to the agent's belief factory.
            agent.beliefFactory.AddBelief("AgentMoving", beliefAgentMoving.Evaluate);
            agent.beliefFactory.AddBelief("AgentIdle", beliefAgentIdle.Evaluate);
            agent.beliefFactory.AddBelief("Nothing", beliefNothing.Evaluate);
            
            /// ------------------------ACTIONS------------------------///
            builder.AddAction(() =>
                new AgentAction.Builder("Relax")
                    .WithStrategy(new IdleStrategy(5)) // This action will make the agent relax for a certain amount of time.
                    .AddEffect(beliefNothing) // This action will set the beliefNothing to true, meaning the agent is doing nothing.
                    .Build());

            builder.AddAction(() =>
                new AgentAction.Builder("Wander Around")
                    .WithStrategy(new WanderStrategy(agent.GetComponent<NavMeshAgent>(), 5)) // This action will make the agent wander around.
                    .AddEffect(beliefAgentMoving) // This action will set the beliefAgentMoving to true, meaning the agent is moving.
                    .Build());
            
            /// ------------------------GOALS------------------------///
            builder.AddGoal(() =>
                new AgentGoal.Builder("Chill Out")
                    .WithPriority(1)
                    .WithDesiredEffect(beliefNothing) // This goal will be achieved when the agent is doing nothing.
                    .Build());

            builder.AddGoal(() =>
                new AgentGoal.Builder("Wander")
                    .WithPriority(1)
                    .WithDesiredEffect(beliefAgentMoving) // This goal will be achieved when the agent is moving.
                    .Build());
            
            // Configure the agent with the built capability.
            builder.Build().Configure(agent);
        }
    }
}
