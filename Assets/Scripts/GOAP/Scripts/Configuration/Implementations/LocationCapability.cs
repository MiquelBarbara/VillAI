using System.Collections;
using GOAP.Scripts;
using GOAP.Scripts.Configuration.Capabilities;
using UnityEngine;

namespace Game.GOAP.Scripts.Configuration.Implementations
{
    public class LocationCapability: ICapabilityConfig
    {
        public void Configure(GoapAgent agent)
        {
            var builder = new CapabilityBuilder("LocationCapability");
            var destinationKey = agent.BlackboardController.GetBlackboard().GetOrRegisterKey("Destination");
            
            agent.beliefFactory.AddBeliefWithPrediction("HaveDestination", () =>
            {
                agent.BlackboardController.GetBlackboard().TryGetValue(destinationKey, out Transform transform);

                return transform != null;
            }, ()=> SetDestination(agent), "Where the agent wants to go?");
            
            
            
            agent.beliefFactory.AddBelief("IAmThere", () =>
            {
                agent.BlackboardController.GetBlackboard().TryGetValue(destinationKey, out Transform transform);
                if (transform == null) return false;
                var myTransform = agent.transform;
                return Vector3.Distance(transform.position , myTransform.position) > 2;
            });
            
            builder.AddAction(() =>
                new AgentAction.Builder("Relax")
                    .AddPrecondition(agent.beliefs["HaveDestination"])
                    .WithStrategy(new MoveStrategy(agent.gameObject, agent.BlackboardController.GetBlackboard(), destinationKey))
                    .AddEffect(agent.beliefs["IAmThere"]) 
                    .Build());
            
            builder.AddGoal(() =>
                new AgentGoal.Builder("Wander")
                    .WithPriority(1)
                    .WithDesiredEffect(agent.beliefs["IAmThere"]) 
                    .Build());
            
        }

        public IEnumerator SetDestination(GoapAgent agent)
        {
            yield break;
        }
    }
}