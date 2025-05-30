using EntitiesRelated.Core;
using GameplayFocused.TimeManagment;
using GOAP.Scripts.Configuration.Capabilities;
using UnityEngine;

namespace GOAP.Scripts.Configuration.Implementations
{


    public class WorkCapabilityConfig : ICapabilityConfig
    {
        public void Configure(GoapAgent agent)
        {
            var builder = new CapabilityBuilder("WorkCapability");
            var blackboard = agent.BlackboardController.GetBlackboard();
            
            var storeLocationKey = blackboard.GetOrRegisterKey("StoreLocation");
            blackboard.TryGetValue(storeLocationKey, out Transform storeLocation);
            
            //agent.beliefFactory.AddBeliefWithPrediction("AgentNeedsMoney", () => true,"Does the agent need money?");
            agent.beliefFactory.AddBelief("AgentHasSufficientMoney", () => !agent.beliefs["AgentNeedsMoney"].Evaluate());
            
            //agent.beliefFactory.AddBeliefWithPrediction("AgentNeedsWork", () => true, "Does the agent still wants to work at the shop?");
            
            agent.beliefFactory.AddLocationBelief("AgentAtWork", 2f, storeLocation);
            
            builder.AddAction(() =>
                new AgentAction.Builder("GoToWork")
                    .WithCost(1)
                    .AddEffect(agent.beliefs["AgentAtWork"])
                    .WithStrategy(new MoveToStrategy(agent.gameObject, agent.beliefs["AgentAtWork"]))
                    .Build());
            
            builder.AddAction(() =>
                new AgentAction.Builder("WorkAtStore")
                    .WithCost(2)
                    .AddPrecondition(agent.beliefs["AgentAtWork"])
                    .AddPrecondition(agent.beliefs["AgentNeedsWork"])
                    .AddEffect(agent.beliefs["AgentHasSufficientMoney"])
                    .WithStrategy(new StoreStrategy(agent.gameObject, ()=> !agent.beliefs["AgentNeedsWork"].Evaluate())) 
                    .Build());
            
            builder.AddGoal(() =>
                new AgentGoal.Builder("EarnMoney")
                    .WithPriority(5)
                    .WithDesiredEffect(agent.beliefs["AgentHasSufficientMoney"]).Build())
                    .Build();

            builder.Build().Configure(agent);
        }
    }
}