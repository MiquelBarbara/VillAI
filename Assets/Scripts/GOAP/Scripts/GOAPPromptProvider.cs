using System.Collections.Generic;
using LLM.Templates;
using NPCs;
using Systems.SaveSystem.Memory;
using UnityEngine;

namespace GOAP.Scripts
{
    
    public class TalkToData : DataTransferObject
    {
        [Input] public List<ComplexCharacterData> targets;
        [Input] public RelationshipsContainer Relationships;
        
        [Output] public string targetName;
    }
    
    public class GOAPPromptProvider: Singleton<GOAPPromptProvider>
    {
        [Header("Prompts for GOAP Actions")]
        [SerializeField] public PromptDefinition talkPromptDefinition;
        [SerializeField] PromptDefinition earnMoneyPromptDefinition;
        [SerializeField] public PromptDefinition expressPromptDefinition;
        
        [Header("AI GOAP Prompts")]
        [SerializeField] public PromptDefinition planPrompt;
        [SerializeField] public PromptDefinition goalPrompt;
        [SerializeField] public PromptDefinition beliefsPrompt;
        
        [Header("AI GOAP beliefs Prompts")]
        [SerializeField] public PromptDefinition moneyBeliefPrompt;
        [SerializeField] public PromptDefinition inventoryBeliefPrompt;
        
    }
}