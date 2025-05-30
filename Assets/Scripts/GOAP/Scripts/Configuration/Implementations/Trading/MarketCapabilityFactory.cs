using System.Collections;
using EntitiesRelated.Interaction.Interactables.Implementations;
using Game.ALPHA;
using GOAP.Scripts;
using GOAP.Scripts.Configuration.Capabilities;
using LLM.Services;
using LLM.Templates;
using UnityServiceLocator;
using Utilities.ScriptableObjectExtensions;

namespace Game.GOAP.Scripts.Configuration.Implementations
{


    public class MarketCapabilityConfig : ICapabilityConfig
    {
        public void Configure(GoapAgent agent)
        {
            var builder = new CapabilityBuilder("MarketCapability");
            var blackboard = agent.BlackboardController.GetBlackboard();
            
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
            
            agent.beliefFactory.AddBelief("AgentWantsToBuy", () => false);

            agent.beliefFactory.AddBelief("AgentWantsToSell", () => false);
            
            builder.AddAction(() =>
                new AgentAction.Builder("GoToMarket")
                    .WithCost(1)
                    .AddEffect(agent.beliefs["AgentNearStore"])
                    .WithStrategy(new MoveToStrategy(agent.gameObject, agent.beliefs["AgentNearStore"]))
                    .Build());
            
            builder.AddAction(() =>
                new AgentAction.Builder("BuyItem")
                    .WithCost(2)
                    .AddPrecondition(agent.beliefs["AgentWantsToBuy"])
                    .AddPrecondition(agent.beliefs["AgentNearStore"])
                    .WithStrategy(new BuyStrategy(3f))
                    .Build());
            
            builder.AddAction(() =>
                new AgentAction.Builder("SellItem")
                    .WithCost(1)
                    .AddPrecondition(agent.beliefs["AgentWantsToSell"])
                    .AddPrecondition(agent.beliefs["AgentNearStore"])
                    .WithStrategy(new SellStrategy(3f))
                    .Build());
            
            builder.AddGoal(() =>
                new AgentGoal.Builder("MarketTransactions")
                    .WithPriority(4)
                    .WithDesiredEffect(agent.beliefs["AgentHasSufficientMoney"])
                    .Build());

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

            if (data.neddedMoney <= 0) yield break;
            agent.BlackboardController.GetBlackboard().SetValue(moneyWantedKey, data.neddedMoney);
        }
    }
    
    public class MoneyData: DataTransferObject 
    {
        [Input] public int currentMoney;
        [Input] public int currentNeededMoney;
        
        [Output] public int neddedMoney;
    }
}
