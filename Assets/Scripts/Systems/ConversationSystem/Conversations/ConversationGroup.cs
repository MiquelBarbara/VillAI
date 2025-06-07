using System;
using System.Collections.Generic;
using HistoryManagment;
using Systems.SaveSystem.Memory.Conversations;
using UnityEditor;
using System.IO;
using System.Linq;
using UnityEngine;

[Serializable]
public class ConversationGroup : ScriptableObject
{
    public string withCharacter;
    [SerializeField] List<Conversation> entries;

    public void AddConversation(List<ConversationEntry> conversation)
    {
        entries ??= new List<Conversation>();
        Conversation conversationAsset = CreateConversationRuntime(conversation);

#if UNITY_EDITOR
        AddToAsset(conversationAsset);
#endif

        conversationAsset.Save();
    }

    public List<string> GetAllSumaries()
    {
        return entries.Select(entry => entry.GetSummary()).ToList();
    }

    private Conversation CreateConversationRuntime(List<ConversationEntry> conversationEntries)
    {
        Conversation conversation = ScriptableObject.CreateInstance<Conversation>();
        conversation.entries = conversationEntries ?? new List<ConversationEntry>();
        conversation.name = $"Conversation_{Guid.NewGuid().ToString()[0..8]}";
        entries.Add(conversation);

        string character1 = conversationEntries[0].GetSpeaker();
        string character2 = conversationEntries[1].GetSpeaker();

        string resolvedCharacter = character1 == withCharacter ? character2 : character1;
        conversation.subFolder = Path.Combine("Characters", resolvedCharacter, "Conversations", withCharacter);

        return conversation;
    }

#if UNITY_EDITOR
    private void AddToAsset(Conversation conversation)
    {
        AssetDatabase.AddObjectToAsset(conversation, this);
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }

    public void OnValidate()
    {
        EditorUtility.SetDirty(this);
    }

    public void RemoveConversation(Conversation conversation)
    {
        entries.Remove(conversation);
        AssetDatabase.RemoveObjectFromAsset(conversation);
        DestroyImmediate(conversation, true);
        AssetDatabase.SaveAssets();
    }
#endif
}