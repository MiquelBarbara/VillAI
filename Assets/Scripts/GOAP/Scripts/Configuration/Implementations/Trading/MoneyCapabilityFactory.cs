using System.Collections;
using GOAP.Scripts;
using GOAP.Scripts.Configuration.Capabilities;
using LLM.Services;
using LLM.Templates;
using UnityServiceLocator;
using Utilities.ScriptableObjectExtensions;

namespace Game.GOAP.Scripts.Configuration.Implementations
{
    
    public class MoneyCapabilityConfig : ICapabilityConfig
    {
        public void Configure(GoapAgent agent)
        {
            var builder = new CapabilityBuilder("MoneyCapability");
            
            agent.beliefFactory.AddBeliefWithPrediction("AgentNeedsMoney", () =>
            {
                var currency = agent.GetComponent<ResourceLocator>().Get<ItemContainer>().GetCurrency();
                var moneyWantedKey = agent.BlackboardController.GetBlackboard().GetOrRegisterKey("MoneyWanted");
                agent.BlackboardController.GetBlackboard().TryGetValue(moneyWantedKey, out int moneyWanted);
                return currency < moneyWanted;
            }, () => NeedMoneyCoroutine(agent), "Does the agent need money?");
            
            
            
            agent.beliefFactory.AddBelief("AgentHasSufficientMoney", () => !agent.beliefs["AgentNeedsMoney"].Evaluate());

            builder.AddAction(() =>
                new AgentAction.Builder("BuyItem")
                    .WithCost(2)
                    .AddPrecondition(agent.beliefs["AgentHasSufficientMoney"])
                    .AddPrecondition(agent.beliefs["AgentNearStore"])
                    .WithStrategy(new BuyStrategy(3f))
                    .Build()
            );
            
            builder.AddAction(() =>
                new AgentAction.Builder("SellItem")
                    .WithCost(1)
                    .AddPrecondition(agent.beliefs["AgentHasSufficientMoney"])
                    .AddPrecondition(agent.beliefs["AgentNearStore"])
                    .AddEffect(agent.beliefs["AgentHasSufficientMoney"])
                    .WithStrategy(new SellStrategy(3f))
                    .Build()
            );
            
            builder.AddGoal(() =>
                new AgentGoal.Builder("AccumulateMoney")
                    .WithPriority(5)
                    .WithDesiredEffect(agent.beliefs["AgentHasSufficientMoney"])
                    .Build()
            );

            builder.Build().Configure(agent);
        }
        
        public IEnumerator NeedMoneyCoroutine(GoapAgent agent)
        {
            var data = new MoneyData()
            {
                currentMoney = agent.GetComponent<ResourceLocator>().Get<ItemContainer>().GetCurrency()
            };
        
            yield return ServiceLocator.Global.Get<IPredictionService>().Predict(GOAPPromptProvider.Instance.moneyBeliefPrompt, data);

            if (data.neddedMoney <= 0) yield break;
            var moneyWantedKey = agent.BlackboardController.GetBlackboard().GetOrRegisterKey("MoneyWanted");
            agent.BlackboardController.GetBlackboard().SetValue(moneyWantedKey, data.neddedMoney);
        }

    }
    
}