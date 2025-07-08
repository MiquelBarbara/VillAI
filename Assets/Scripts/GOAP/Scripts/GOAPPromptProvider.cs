using System.Collections;
using System.Collections.Generic;
using LLM.Services;
using LLM.Templates;
using NPCs;
using Systems.SaveSystem.Memory;
using UnityEngine;
using UnityServiceLocator;
using Utilities.ScriptableObjectExtensions;

namespace GOAP.Scripts
{
    
    public class TalkToData : DataTransferObject
    {
        [Input] public List<ComplexCharacterData> targets;
        [Input] public RelationshipsContainer Relationships;
        
        [Output] public string targetName;
    }
    
    public class GenericQuestionData: DataTransferObject 
    {
        [Input] public string question;
        [Input] public ComplexCharacterData character;
        
        [Output] public bool condition;
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
        [SerializeField] public PromptDefinition questionPrompt;
        
        public static IEnumerator GenericQuestion(GoapAgent agent, string beliefName, string question)
        {
            var data = new GenericQuestionData
            {
                question = question,
                character = agent.GetComponent<ResourceLocator>().Get<ComplexCharacterData>()
            };

            yield return ServiceLocator.Global.Get<IPredictionService>().Predict(GOAPPromptProvider.Instance.questionPrompt, data);
            
            agent.beliefs[beliefName].NewCondition(()=> data.condition);
        }
        
    }
}