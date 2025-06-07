using System.Collections.Generic;
using HistoryManagment;
using Systems.SaveSystem.Memory.Conversations;
using UnityEditor;
using UnityEngine;
using Utilities.ScriptableObjectExtensions;

namespace Systems.SaveSystem.Memory
{
    [CreateAssetMenu(fileName = "ConversationContainer", menuName = "Resources/Conversations/ConversationContainer")]
    public class ConversationContainer : ScriptableSave
    {
        [SerializeField] private List<ConversationGroup> conversations = new();

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
                group = CreateGroupRuntime(characterName);
                conversations.Add(group);

                #if UNITY_EDITOR
                AssetDatabase.AddObjectToAsset(group, this);
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
                #endif
            }

            group.AddConversation(conversation);

            #if UNITY_EDITOR
            EditorUtility.SetDirty(group);
            AssetDatabase.SaveAssets();
            #endif
        }

        public ConversationGroup CreateGroupRuntime(string characterName)
        {
            ConversationGroup group = ScriptableObject.CreateInstance<ConversationGroup>();
            group.name = $"Group_{characterName}";
            group.withCharacter = characterName;
            return group;
        }

        #if UNITY_EDITOR
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