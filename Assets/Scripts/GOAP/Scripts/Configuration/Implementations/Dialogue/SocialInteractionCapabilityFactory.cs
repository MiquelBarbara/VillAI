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
            
            agent.beliefFactory.AddBeliefWithPrediction("WantToSocialize", () => false, () => GOAPPromptProvider.GenericQuestion(agent, "WantToSocialize", "Do I want to talk with someone?"));
            
            var npcSensor = new TargetSensor<TalkInteract>(agent.transform, 5, 100f);
            
            agent.RegisterSensor(npcSensor);
            
            agent.beliefFactory.AddBelief("HasSelectedTarget", () => npcSensor.CurrentTarget != null);
            agent.beliefFactory.AddSensorBelief("AvailableAgent", npcSensor);
            
            agent.beliefFactory.AddBelief("NearbyAgent", () => npcSensor.IsTargetInRange);
            
            agent.beliefFactory.AddBelief("SocializationIsHigh", () => !agent.beliefs["WantToSocialize"].Evaluate());
            
            builder.AddAction(() =>
                new AgentAction.Builder("SelectSocialTarget")
                    .WithCost(0.5f)
                    .AddPrecondition(agent.beliefs["WantToSocialize"])
                    .AddEffect(agent.beliefs["HasSelectedTarget"])
                    .WithStrategy(new SelectWhoToTalk(agent, npcSensor))
                    .Build());
            
            builder.AddAction(() =>
                new AgentAction.Builder("MoveToAgent")
                    .WithCost(1)
                    .AddPrecondition(agent.beliefs["WantToSocialize"])
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
