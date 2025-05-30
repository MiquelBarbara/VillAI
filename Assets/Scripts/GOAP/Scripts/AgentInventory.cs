using System;
using System.Collections;
using System.Collections.Generic;
using LLM.Services;
using LLM.Templates;
using UnityEngine;
using UnityServiceLocator;
using Utilities.ScriptableObjectExtensions;

namespace GOAP.Scripts
{

    public class InventoryPredictionData: DataTransferObject
    {
        [Input] public List<Item> availableItems;
        [Input] public Dictionary<string, int> inventory;
        
        [Output] public Dictionary<string, int> requiredItems;
    }
    
    
    public class AgentInventory: Singleton<MonoBehaviour>
    {
        [SerializeField] private PromptDefinition itemContainerPredict;
        [SerializeField] private ItemDatabase itemDatabase;
        
        public IEnumerator PredictInventoryBelief(GoapAgent agent)
        {
            
            var inventoryData = new InventoryPredictionData
            {
                availableItems = itemDatabase.Items,
                inventory = agent.GetComponent<ResourceLocator>().Get<ItemContainer>().GetAmountByItem()
            };

            yield return ServiceLocator.Global.Get<IPredictionService>().Predict(itemContainerPredict, inventoryData);
            
            var result = inventoryData.requiredItems;
            
            var itemsNeededKey = agent.BlackboardController.GetBlackboard().GetOrRegisterKey("ItemsNeeded");
            agent.BlackboardController.GetBlackboard().SetValue(itemsNeededKey, inventoryData.requiredItems);
        }
    }
}