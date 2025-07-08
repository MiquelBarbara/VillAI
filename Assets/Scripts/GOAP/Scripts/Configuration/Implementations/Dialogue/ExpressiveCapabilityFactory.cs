using System.Collections;
using System.Collections.Generic;
using Game.ALPHA;
using GOAP.Configuration;
using GOAP.Scripts;
using GOAP.Scripts.AgentStats;
using GOAP.Scripts.AgentStats.Strategies;
using GOAP.Scripts.Configuration.Capabilities;
using LLM.Services;
using NPCs;
using UI;
using UnityEngine;
using UnityServiceLocator;
using Utilities.ScriptableObjectExtensions;
using Random = System.Random;

namespace Game.GOAP.Scripts.Configuration.Implementations
{
    public class ExpressiveCapabilityConfig : ICapabilityConfig
    {
        public void Configure(GoapAgent agent)
        {
            var builder = new CapabilityBuilder("ExpressiveCapability");
            var statSystem = agent.GetComponent<StatSystem>();
            
            agent.beliefFactory.AddBeliefWithPrediction("WantToExpress", () => false, () => GOAPPromptProvider.GenericQuestion(agent, "WantToExpress", "Do I want to express something?"));
            
            agent.beliefFactory.AddBelief("Relieved", () => !agent.beliefs["WantToExpress"].Evaluate());
            
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
