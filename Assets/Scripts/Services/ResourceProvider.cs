using System.Collections.Generic;
using LLM.Tools;
using Systems.SaveSystem.Memory;
using Systems.SaveSystem.Memory.Conversations;
using UnityEngine;
using UnityServiceLocator;
using Utilities.ScriptableObjectExtensions;

namespace Services
{
    public class ToolNPCProvider : MonoBehaviour
    {
        [ToolMethod("get_thoughts", "Retrieves thoughts memory for a given NPC ID")]
        public Thoughts GetThoughts(string npcId)
        {
            var memory = ServiceLocator.Global.Get<IResourceService>().GetResource<Thoughts>(npcId);
            if (memory != null) return memory;
            Debug.LogWarning($"No Thoughts memory found for NPC with ID: {npcId}");
            return null;
        }

        [ToolMethod("get_knowledge", "Retrieves knowledge memory for a given NPC ID")]
        public KnowledgeContainer GetKnowledge(string npcId)
        {
            var memory = ServiceLocator.Global.Get<IResourceService>().GetResource<KnowledgeContainer>(npcId);
            if (memory != null) return memory;
            Debug.LogWarning($"No Knowledge memory found for NPC with ID: {npcId}");
            return null;
        }

        [ToolMethod("get_conversation",
            "Retrieves conversations memory for a given NPC ID with other NPC. Pass two NPC IDs")]
        public List<string> GetConversation(string npcId, string otherNpcId)
        {
            var memory = ServiceLocator.Global.Get<IResourceService>().GetResource<ConversationContainer>(npcId);
            var convesationGroup = memory.FilterByCharacter(otherNpcId);
            if (memory != null) return convesationGroup.GetAllSumaries();
            Debug.LogWarning($"No Conversation memory found for NPC with ID: {npcId}");
            return null;
        }

        [ToolMethod("get_inventory", "Retrieves inventory memory for a given NPC ID")]
        public ItemContainer GetInventory(string npcId)
        {
            var memory = ServiceLocator.Global.Get<IResourceService>().GetResource<ItemContainer>(npcId);
            if (memory != null) return memory;
            Debug.LogWarning($"No Inventory memory found for NPC with ID: {npcId}");
            return null;
        }

        [ToolMethod("sum_numbers", "Test method to check if the provider is working")]
        public float Test(int number1, int number2)
        {
            Debug.Log($"Test method called with numbers: {number1}, {number2}");
            return number1 + number2;
        }
    }
}