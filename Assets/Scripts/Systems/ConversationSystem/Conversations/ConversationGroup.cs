using System;
using System.Collections.Generic;
using HistoryManagment;
using Systems.SaveSystem.Memory.Conversations;
using UnityEditor;
using System.IO;
using System.Linq;
using UnityEngine;

[Serializable]
public class ConversationGroup: ScriptableObject
{
    public string withCharacter;
    [SerializeField] List<Conversation> entries;
    
    public void AddConversation(List<ConversationEntry> conversation)
    {
        entries ??= new List<Conversation>();
        Conversation conversationAsset = CreateConversation(conversation);
        conversationAsset.Save();
    }
    
    public List<string> GetAllSumaries()
    {
        return entries.Select(entry => entry.GetSummary()).ToList();
    }
    
    #if UNITY_EDITOR

    public void OnValidate()
    {
        EditorUtility.SetDirty(this);
    }
    public Conversation CreateConversation(List<ConversationEntry> conversationEntries = null) {
        Conversation conversation = CreateInstance<Conversation>();
        conversation.entries = conversationEntries ?? new List<ConversationEntry>();
        conversation.name = $"Conversation_{Guid.NewGuid().ToString()[0..8]}";
        entries.Add(conversation);
        
        string character1 = conversationEntries[0].GetSpeaker();
        string character2 = conversationEntries[1].GetSpeaker();
        
        string resolvedCharacter = character1;

        if (character1 == withCharacter)
        {
            resolvedCharacter = character2;
        }
        
        conversation.subFolder = Path.Combine("Characters", resolvedCharacter , "Conversations", withCharacter);
        
        AssetDatabase.AddObjectToAsset(conversation, this); 
        AssetDatabase.SaveAssets();
        return conversation;
    }
    
    public void RemoveConversation(Conversation conversation) {
        entries.Remove(conversation);
        AssetDatabase.RemoveObjectFromAsset(conversation);
        DestroyImmediate(conversation, true);
        AssetDatabase.SaveAssets();
    }
    #endif
}