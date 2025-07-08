using System.Collections;
using System.Collections.Generic;
using GameplayFocused;
using GOAP.Scripts.Configuration.Capabilities;
using LLM.Services;
using LLM.Templates;
using UnityServiceLocator;
using Utilities.ScriptableObjectExtensions;

namespace GOAP.Scripts.Configuration.Implementations.Trading
{
    public class InventoryCapability: ICapabilityConfig
    {
        public void Configure(GoapAgent agent)
        {
            var builder = new CapabilityBuilder("InventoryCapability");
            var blackboard = agent.BlackboardController.GetBlackboard();
            
            agent.beliefFactory.AddBeliefWithPrediction("AgentWantsItems", () =>
            {
                var myItems = agent.GetComponent<ResourceLocator>().Get<ItemContainer>().GetAmountByItem();
                
                var itemsWantedKey = agent.BlackboardController.GetBlackboard().GetOrRegisterKey("ItemsWanted");
                agent.BlackboardController.GetBlackboard().TryGetValue(itemsWantedKey, out Dictionary<string, int> itemsWanted);
                
                foreach (var item in itemsWanted)
                {
                    if (!myItems.TryGetValue(item.Key, out var amount) || (int)item.Value > (int)amount)
                    {
                        return true;
                    }
                }
                return false;
            }, () => InventoryPrediction(agent), "Does the agent need items?");
            
            agent.beliefFactory.AddBelief("AgentHasSufficientItems", () => !agent.beliefs["AgentWantsItems"].Evaluate());
            
            builder.AddGoal(() =>
                new AgentGoal.Builder("GetItems")
                    .WithPriority(4)
                    .WithDesiredEffect(agent.beliefs["AgentHasSufficientItems"])
                    .Build());

            builder.Build().Configure(agent);
        }

        public IEnumerator InventoryPrediction(GoapAgent agent)
        {
            var moneyWantedKey = agent.BlackboardController.GetBlackboard().GetOrRegisterKey("ItemsWanted");
            agent.BlackboardController.GetBlackboard().TryGetValue(moneyWantedKey, out Dictionary<string, int> itemsWanted);
            
            var inventoryData = new InventoryData
            {
                availableItems = GameManager.Instance.GetItemDatabase().Items,
                inventory = agent.GetComponent<ResourceLocator>().Get<ItemContainer>().GetAmountByItem(),
                itemsWanted = itemsWanted
            };

            yield return ServiceLocator.Global.Get<IPredictionService>().Predict(GOAPPromptProvider.Instance.inventoryBeliefPrompt, inventoryData);
            
            var result = inventoryData.requiredItems;
            
            var itemsNeededKey = agent.BlackboardController.GetBlackboard().GetOrRegisterKey("ItemsNeeded");
            agent.BlackboardController.GetBlackboard().SetValue(itemsNeededKey, inventoryData.requiredItems);
        }
    }
    
    public class InventoryData: DataTransferObject 
    {
        [Input] public List<Item> availableItems;
        [Input] public Dictionary<string, int> inventory;
        [Input] public Dictionary<string, int> itemsWanted;
        
        [Output] public Dictionary<string, int> requiredItems;
    }
}