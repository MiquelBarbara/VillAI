using System;
using System.Collections.Generic;
using EntitiesRelated.Core.Data;
using LLM.Templates;
using Systems.SaveSystem.Memory;
using UnityEngine;

public class MoodTemplateData : DataTransferObject
{
    [Input] public List<string> available_moods;
    [Input] public CharacterData PassiveCharacter;
    [Input] public String message;
    [Input] public CharacterData ActiveCharacter;
    [Input] public Mood emotional_state;
    [Input] public Relationships relationship;
    
    
    [Output] public Mood mood;
    [Output] public System.String feelings;
}