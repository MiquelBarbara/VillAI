using System.Collections.Generic;
using LLM.Tools;
using Systems.SaveSystem.Memory;
using Systems.SaveSystem.Memory.Conversations;
using UnityEngine;
using UnityServiceLocator;
using Utilities.ScriptableObjectExtensions;

namespace Services
{
    /// <summary>
    /// ToolNPCProvider is a MonoBehaviour that provides methods to retrieve various types of memory for NPCs.
    /// </summary>
    public class ToolNPCProvider : MonoBehaviour
    {
        /// <summary>
        /// Retrieves thoughts memory for a given NPC ID.
        /// </summary>
        /// <param name="npcId"> The ID of the NPC whose thoughts memory is to be retrieved.</param>
        /// <returns> Returns the Thoughts memory associated with the NPC ID, or null if not found.</returns>
        [ToolMethod("get_thoughts", "Retrieves thoughts memory for a given NPC ID")]
        public Thoughts GetThoughts(string npcId)
        {
            var memory = ServiceLocator.Global.Get<IResourceService>().GetResource<Thoughts>(npcId);
            if (memory != null) return memory;
            Debug.LogWarning($"No Thoughts memory found for NPC with ID: {npcId}");
            return null;
        }

        
        /// <summary>
        /// Retrieves knowledge memory for a given NPC ID.
        /// </summary>
        /// <param name="npcId"> The ID of the NPC whose knowledge memory is to be retrieved.</param>
        /// <returns> Returns the KnowledgeContainer memory associated with the NPC ID, or null if not found.</returns>
        [ToolMethod("get_knowledge", "Retrieves knowledge memory for a given NPC ID")]
        public KnowledgeContainer GetKnowledge(string npcId)
        {
            var memory = ServiceLocator.Global.Get<IResourceService>().GetResource<KnowledgeContainer>(npcId);
            if (memory != null) return memory;
            Debug.LogWarning($"No Knowledge memory found for NPC with ID: {npcId}");
            return null;
        }

        /// <summary>
        /// Retrieves conversations memory for a given NPC ID with another NPC.
        /// </summary>
        /// <param name="npcId"> The ID of the NPC whose conversations memory is to be retrieved.</param>
        /// <param name="otherNpcId"> The ID of the other NPC involved in the conversation.</param>
        /// <returns> Returns a list of conversation summaries between the two NPCs, or null if not found.</returns>
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

        /// <summary>
        /// Retrieves inventory memory for a given NPC ID.
        /// </summary>
        /// <param name="npcId"> The ID of the NPC whose inventory memory is to be retrieved.</param>
        /// <returns> Returns the ItemContainer memory associated with the NPC ID, or null if not found.</returns>
        [ToolMethod("get_inventory", "Retrieves inventory memory for a given NPC ID")]
        public ItemContainer GetInventory(string npcId)
        {
            var memory = ServiceLocator.Global.Get<IResourceService>().GetResource<ItemContainer>(npcId);
            if (memory != null) return memory;
            Debug.LogWarning($"No Inventory memory found for NPC with ID: {npcId}");
            return null;
        }

        /// <summary>
        /// Test method to check if the provider is working.
        /// </summary>
        /// <param name="number1"> The first number to sum.</param>
        /// <param name="number2"> The second number to sum.</param>
        /// <returns> Returns the sum of the two numbers.</returns>
        [ToolMethod("sum_numbers", "Test method to check if the provider is working")]
        public float Test(int number1, int number2)
        {
            Debug.Log($"Test method called with numbers: {number1}, {number2}");
            return number1 + number2;
        }
    }
}