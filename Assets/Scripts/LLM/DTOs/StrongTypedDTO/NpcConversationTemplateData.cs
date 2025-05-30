using System;
using System.Collections.Generic;
using EntitiesRelated.Core.Data;
using HistoryManagment;
using LLM.Templates;
using Systems.DialogueSystem.Moods;
using Systems.SaveSystem.Memory;
using UnityEngine;


public class NpcConversationTemplateData : DataTransferObject
{
    [Input] public CharacterData PassiveCharacter;
    [Input] public String message;
    [Input] public CharacterData ActiveCharacter;
    [Input] public Mood emotional_state;
    [Input] public Relationships relationship;
    [Input] public List<ConversationEntry> history;
    [Input] public List<string> otherConversations;
    
    
    [Output] public String npc_response;
    [Output] public String internal_thoughts;
    [Output] public String knowledge;
}