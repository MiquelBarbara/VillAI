using Game.ALPHA;
using GOAP.Configuration;
using GOAP.Scripts;
using GOAP.Scripts.AgentStats;
using GOAP.Scripts.AgentStats.Strategies;
using GOAP.Scripts.Configuration.Capabilities;
using UI;
using UnityEngine;

namespace Game.GOAP.Scripts.Configuration.Implementations
{

    
    public class ExpressiveCapabilityConfig : ICapabilityConfig
    {
        public void Configure(GoapAgent agent)
        {
            var builder = new CapabilityBuilder("ExpressiveCapability");
            var statSystem = agent.GetComponent<StatSystem>();
            
            //agent.beliefFactory.AddBeliefWithPrediction("WantToExpress", () => false, "Does the agent want to express something to the world?");
            
            Stat relive;
            if (!statSystem.TryGetStat("Relive", out relive))
            {
                var builderStat = new StatBuilderConfig()
                    .SetKey("Relive")
                    .SetInitialValue(100f)
                    .SetThresholds(30f, 50f)
                    .SetDecayStrategy(new LinearDecayStrategy(1f))
                    .SetIncreaseStrategy(new CappedIncreaseStrategy(80f))
                    .SetDecreaseStrategy(new GradualDecreaseStrategy(1f))
                    .Build();

                builderStat.Configure(statSystem);
                statSystem.TryGetStat("Relive", out relive);
            }
            
            agent.beliefFactory.AddBelief("WantToExpress", () => relive.IsLow);
            agent.beliefFactory.AddBelief("Relieved", () => relive.IsHigh);
            
            var expressPoints =
                new TargetSensor<ReactableObject>(agent.transform, 5, 10);
            agent.RegisterSensor(expressPoints);
            
            agent.beliefFactory.AddSensorBelief("NearExpressPoint", expressPoints);
            
            
            builder.AddAction(() =>
                new AgentAction.Builder("ExpressByPoint")
                    .WithStrategy(new ExpressionStrategy(agent, expressPoints.detectedObjects))
                    .AddEffect(agent.beliefs["Relieved"])
                    .Build());
            
            builder.AddGoal(() =>
                new AgentGoal.Builder("Express")
                    .WithDescription("Say something to the world")
                    .WithPriority(2)
                    .WithDesiredEffect(agent.beliefs["Relieved"])
                    .Build());
            
            builder.Build().Configure(agent);
        }
    }
}
