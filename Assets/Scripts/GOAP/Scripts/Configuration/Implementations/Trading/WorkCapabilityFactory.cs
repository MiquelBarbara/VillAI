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
                    .AddPrecondition(agent.beliefs["AgentNeedsMoney"])
                    .AddEffect(agent.beliefs["AgentHasSufficientMoney"])
                    .WithStrategy(new StoreStrategy(agent.gameObject, ()=> agent.beliefs["AgentHasSufficientMoney"].Evaluate())) 
                    .Build());
            
            builder.AddGoal(() =>
                new AgentGoal.Builder("EarnMoney")
                    .WithPriority(5)
                    .WithDesiredEffect(agent.beliefs["AgentHasSufficientMoney"])
                    .Build());

            builder.Build().Configure(agent);
        }
    }
}