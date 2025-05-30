using System;
using System.Collections.Generic;
using HistoryManagment;
using LLM.Templates;
using NPCs;
using Systems.SaveSystem.Memory;
using UnityEngine;

public class UpdateRelationshipTemplateData : DataTransferObject
{
    [Input] public ComplexCharacterData character1;
    [Input] public ComplexCharacterData character2;
    [Input] public Relationships relationship;
    [Input] public List<ConversationEntry> entries;
    [Input] public List<string> metrics;
    
    
    [Output] public List<RelationshipMetricData> listUpdatedMetrics;
    [Output] public String perception;
}