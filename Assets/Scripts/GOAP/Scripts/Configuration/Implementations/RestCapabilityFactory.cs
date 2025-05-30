
using GOAP.Configuration;
using GOAP.Scripts;
using UnityEngine;
using GOAP.Scripts.AgentStats;
using GOAP.Scripts.AgentStats.Strategies;
using GOAP.Scripts.Configuration.Capabilities;

namespace Game.GOAP.Scripts.Configuration.Implementations
{
    
    public class RestCapabilityConfig : ICapabilityConfig
    {
        public void Configure(GoapAgent agent)
        {
            var builder = new CapabilityBuilder("RestCapability");
            var blackboard = agent.BlackboardController.GetBlackboard();
            
            var homeLocationKey = blackboard.GetOrRegisterKey("HomeLocation");
            
            if (!blackboard.TryGetValue(homeLocationKey, out Transform homePosition))
            {
                Debug.LogWarning("HomeLocation not found in blackboard! Assigning default position.");
                homePosition = agent.transform; // Default to current position if not set
                blackboard.SetValue(homeLocationKey, homePosition);
            }
            var statSystem = agent.GetComponent<StatSystem>();

            Stat energyStat;
            if (!statSystem.TryGetStat("Stamina", out energyStat))
            {
                var statConfig = new StatBuilderConfig()
                    .SetKey("Stamina")
                    .SetInitialValue(100f)
                    .SetThresholds(20f, 70f)
                    .SetDecayStrategy(new ExponentialDecayStrategy(DecayCalculator.CalculateExponentialDecayConstant(100,1,9*60)))
                    .SetIncreaseStrategy(new GradualIncreaseStrategy(5f))
                    .SetDecreaseStrategy(new GradualDecreaseStrategy(5f))
                    .Build();

                statConfig.Configure(statSystem);
                statSystem.TryGetStat("Stamina", out energyStat);
            }
            
            agent.beliefFactory.AddBelief("AgentNeedsRest", () => energyStat.IsLow);
            agent.beliefFactory.AddLocationBelief("AgentAtHome", 2f, homePosition);
            agent.beliefFactory.AddBelief("AgentIsRested", () => energyStat.IsHigh);
            
            builder.AddAction(() =>
                new AgentAction.Builder("GoHomeToRest")
                    .WithCost(1)
                    .AddPrecondition(agent.beliefs["AgentNeedsRest"])
                    .AddEffect(agent.beliefs["AgentAtHome"])
                    .WithStrategy(new MoveStrategy(agent.gameObject, blackboard, homeLocationKey))
                    .Build());

            builder.AddAction(() =>
                new AgentAction.Builder("Rest")
                    .WithCost(1)
                    .AddPrecondition(agent.beliefs["AgentAtHome"])
                    .AddEffect(agent.beliefs["AgentIsRested"])
                    .WithStrategy(new RestStrategy(5f, statSystem))
                    .Build());
            
            builder.AddGoal(() =>
                new AgentGoal.Builder("RestAtHome")
                    .WithPriority(3)
                    .WithDesiredEffect(agent.beliefs["AgentIsRested"])
                    .Build());

            // Apply the capability configuration
            builder.Build().Configure(agent);
        }
    }
}
