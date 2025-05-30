using GOAP.Configuration;
using GOAP.Scripts;
using UnityEngine;
using UnityEngine.AI;
using Blackboard_Architecture;
using EntitiesRelated.Interaction;
using EntitiesRelated.Interaction.Interactables.Implementations;
using Game.ALPHA;
using GOAP.Scripts.AgentStats;
using GOAP.Scripts.AgentStats.Strategies;
using GOAP.Scripts.Configuration.Capabilities;
using GOAP.Scripts.Dialogue;
using NPCs;

namespace Game.GOAP.Scripts.Configuration.Implementations
{
    
    public class SocialInteractionCapabilityConfig : ICapabilityConfig
    {
        public void Configure(GoapAgent agent)
        {
            var builder = new CapabilityBuilder("SocialInteractionCapability");
            var blackboard = agent.BlackboardController.GetBlackboard();
            var statSystem = agent.GetComponent<StatSystem>();
            
            
            
            Stat socializationStat;
            if (!statSystem.TryGetStat("Socialization", out socializationStat))
            {
                var builderStat = new StatBuilderConfig()
                    .SetKey("Socialization")
                    .SetInitialValue(100f)
                    .SetThresholds(30f, 50f)
                    .SetDecayStrategy(new LinearDecayStrategy(1f))
                    .SetIncreaseStrategy(new CappedIncreaseStrategy(80f))
                    .SetDecreaseStrategy(new GradualDecreaseStrategy(1f))
                    .Build();

                builderStat.Configure(statSystem);
                statSystem.TryGetStat("Socialization", out socializationStat);
            }
            
            var npcSensor = new TargetSensor<TalkInteract>(agent.transform, 5, 100f);
            
            agent.RegisterSensor(npcSensor);
            
            agent.beliefFactory.AddBelief("HasSelectedTarget", () => npcSensor.CurrentTarget != null);
            agent.beliefFactory.AddSensorBelief("AvailableAgent", npcSensor);
            
            agent.beliefFactory.AddBelief("NearbyAgent", () => npcSensor.IsTargetInRange);
            
            agent.beliefFactory.AddBelief("AgentWantsToSocialize", () => socializationStat.IsLow);
            agent.beliefFactory.AddBelief("SocializationIsHigh", () => socializationStat.IsHigh);
            agent.beliefFactory.AddBelief("SocializationIsNormal", () => socializationStat.IsNormal);
            
            
            builder.AddAction(() =>
                new AgentAction.Builder("SelectSocialTarget")
                    .WithCost(0.5f)
                    .AddPrecondition(agent.beliefs["AgentWantsToSocialize"])
                    .AddEffect(agent.beliefs["HasSelectedTarget"])
                    .WithStrategy(new SelectWhoToTalk(agent, npcSensor))
                    .Build());
            
            builder.AddAction(() =>
                new AgentAction.Builder("MoveToAgent")
                    .WithCost(1)
                    .AddPrecondition(agent.beliefs["AgentWantsToSocialize"])
                    .AddPrecondition(agent.beliefs["HasSelectedTarget"])
                    .AddEffect(agent.beliefs["NearbyAgent"])
                    .WithStrategy(new MoveToStrategy(agent.gameObject, agent.beliefs["AvailableAgent"]))
                    .Build());

            builder.AddAction(() =>
                new AgentAction.Builder("StartTalking")
                    .WithCost(2)
                    .AddPrecondition(agent.beliefs["NearbyAgent"])
                    .AddEffect(agent.beliefs["SocializationIsHigh"])
                    .WithStrategy(new TalkStrategy(
                        agent.GetComponent<NavMeshAgent>(),
                        agent.GetComponent<InteractController>(),
                        ()=> npcSensor.CurrentTarget))
                    .Build());
            
            builder.AddGoal(() =>
                new AgentGoal.Builder("Socialize")
                    .WithPriority(2)
                    .WithDesiredEffect(agent.beliefs["SocializationIsHigh"])
                    .Build());
            
            builder.Build().Configure(agent);
        }
    }
}
