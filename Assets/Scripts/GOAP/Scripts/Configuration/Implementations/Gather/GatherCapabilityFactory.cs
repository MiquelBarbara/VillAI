using EntitiesRelated.Core;
using Game.ALPHA;
using GameplayFocused.ExtractingResource;
using GOAP.Scripts;
using GOAP.Scripts.Configuration.Capabilities;
using UnityEngine;

namespace Game.GOAP.Scripts.Configuration.Implementations
{


    // The generic capability configuration for gathering objects of type T.
    public class GatherCapabilityConfig<T> : ICapabilityConfig where T : GatherableResource
    {
        public void Configure(GoapAgent agent)
        {
            var builder = new CapabilityBuilder($"GatherCapability_{typeof(T).Name}");
            var blackboard = agent.BlackboardController.GetBlackboard();
            
            var targetSensor = new TargetSensorWithStrategy<T>(agent.transform, 5, 100f, new ProximityTargetSelectionCommand());
            
            agent.RegisterSensor(targetSensor);
            
            string availableBeliefKey = $"{typeof(T).Name}Available";
            agent.beliefFactory.AddSensorBelief(availableBeliefKey, targetSensor);
            
            agent.beliefFactory.AddBelief("HaveGathered", () => false);
            
            string nearTargetBeliefKey = $"Near{typeof(T).Name}";
            agent.beliefFactory.AddBelief(nearTargetBeliefKey, () =>
            {
                if (targetSensor.IsTargetInRange)
                {
                    return Vector3.Distance(agent.transform.position, targetSensor.LastKnownPosition) < 1f;
                }
                return false;
            });
            
            builder.AddAction(() =>
                new AgentAction.Builder($"MoveTo{typeof(T).Name}")
                    .WithCost(1)
                    .AddPrecondition(agent.beliefs[availableBeliefKey])
                    .AddEffect(agent.beliefs[nearTargetBeliefKey])
                    .WithStrategy(new MoveToStrategy(agent.gameObject, agent.beliefs[availableBeliefKey]))
                    .Build()
            );
            
            builder.AddAction(() =>
                new AgentAction.Builder($"Gather{typeof(T).Name}")
                    .WithCost(1)
                    .AddPrecondition(agent.beliefs[nearTargetBeliefKey])
                    .AddEffect(agent.beliefs["HaveGathered"])
                    .WithStrategy(new InteractStrategy(agent.GetComponent<Character>(), () => targetSensor.CurrentTarget, 1f))
                    .Build()
            );
            
            builder.AddGoal(() =>
                new AgentGoal.Builder($"Collect{typeof(T).Name}")
                    .WithDescription($"Objective to gather {typeof(T).Name}")
                    .WithPriority(4)
                    .WithDesiredEffect(agent.beliefs["HaveGathered"])
                    .Build()
            );

            builder.Build().Configure(agent);
        }
    }
}
