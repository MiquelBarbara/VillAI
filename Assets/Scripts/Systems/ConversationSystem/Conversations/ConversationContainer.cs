using System.Collections.Generic;
using HistoryManagment;
using Systems.SaveSystem.Memory.Conversations;
using UnityEditor;
using UnityEngine;
using Utilities.ScriptableObjectExtensions;

namespace Systems.SaveSystem.Memory
{
    [CreateAssetMenu(fileName = "ConversationContainer", menuName = "Resources/Conversations/ConversationContainer")]
    public class ConversationContainer: ScriptableSave
    {
        [SerializeField] List<ConversationGroup> conversations = new();
        
        public ConversationGroup FilterByCharacter(string character)
        {
            return conversations.Find(c => c.withCharacter == character);
        }
        
        public List<string> GetAllSummaries(string characterName)
        {
            return FilterByCharacter(characterName)?.GetAllSumaries();
        }
        
        public void InsertIntoGroup(List<ConversationEntry> conversation, string characterName)
        {
            ConversationGroup group = conversations.Find(c => c.withCharacter == characterName);
            if (group == null)
            {
                group = CreateGroup(characterName);
            }
            group.AddConversation(conversation);
            AssetDatabase.SaveAssets();
        }
        
        
        #if UNITY_EDITOR
        
        public ConversationGroup CreateGroup(string characterName)
        {
            ConversationGroup group = CreateInstance<ConversationGroup>();
            group.name = $"Group_{characterName}";
            group.withCharacter = characterName;
        
            // Add as subasset
            AssetDatabase.AddObjectToAsset(group, this);
            conversations.Add(group);
        
            AssetDatabase.SaveAssets();
            return group;
        }
        
        public ConversationGroup RemoveGroup(string characterName)
        {
            ConversationGroup group = conversations.Find(c => c.withCharacter == characterName);
            if (group == null) return group;
            conversations.Remove(group);
            AssetDatabase.RemoveObjectFromAsset(group);
            DestroyImmediate(group, true);
            AssetDatabase.SaveAssets();
            return group;
        }
        #endif
    }
}