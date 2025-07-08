using System.Collections;
using EntitiesRelated.Interaction.Interactables.Implementations;
using Game.ALPHA;
using GOAP.Scripts;
using GOAP.Scripts.Configuration.Capabilities;
using LLM.Services;
using LLM.Templates;
using UnityEngine;
using UnityServiceLocator;
using Utilities.ScriptableObjectExtensions;

namespace Game.GOAP.Scripts.Configuration.Implementations
{
    public class MoneyData: DataTransferObject 
    {
        [Input] public int currentMoney;
        [Input] public int currentNeededMoney;
        
        [Output] public int neededMoney;
    }
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
            
            var targetSensor =
                new TargetSensorWithStrategy<StoreInteract>(agent.transform, 5,100f, new ProximityTargetSelectionCommand());
            agent.RegisterSensor(targetSensor);

            agent.beliefFactory.AddSensorBelief("AgentNearStore", targetSensor);

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
                    .AddPrecondition(agent.beliefs["AgentNeedsMoney"])
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
            var moneyWantedKey = agent.BlackboardController.GetBlackboard().GetOrRegisterKey("MoneyWanted");
            agent.BlackboardController.GetBlackboard().TryGetValue(moneyWantedKey, out int moneyWanted);
            var data = new MoneyData()
            {
                currentMoney = agent.GetComponent<ResourceLocator>().Get<ItemContainer>().GetCurrency(),
                currentNeededMoney = moneyWanted
            };
        
            yield return ServiceLocator.Global.Get<IPredictionService>().Predict(GOAPPromptProvider.Instance.moneyBeliefPrompt, data);

            if (data.neededMoney <= 0) yield break;
            agent.BlackboardController.GetBlackboard().SetValue(moneyWantedKey, data.neededMoney);
        }

    }
    
}