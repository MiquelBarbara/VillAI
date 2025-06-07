using System.Collections;
using System.Collections.Generic;
using EntitiesRelated.Interaction.Interactables.Implementations;
using LLM.Services;
using LLM.Templates;
using NPCs;
using Systems.SaveSystem.Memory;
using UnityEngine;
using UnityServiceLocator;
using Utilities.ScriptableObjectExtensions;

namespace GOAP.Scripts.Dialogue
{
    
    public class SocialAgentData : DataTransferObject
    {
        [Input] public List<ComplexCharacterData> agents;
        [Input] public List<Relationships> relationships;
        
        [Output] public string characterName;
    }
    
    public class SelectWhoToTalk : CoroutineStrategy
    { 
        private List<TalkInteract> npcs;
        private TargetSensor<TalkInteract> targetSensor;
        public SelectWhoToTalk(MonoBehaviour provider, TargetSensor<TalkInteract> targetSensor) : base(provider)
        {
            this.targetSensor = targetSensor;
        }
        protected override IEnumerator Predict()
        {
            List<ComplexCharacterData> agents = new List<ComplexCharacterData>();
            List<Relationships> relationships = new List<Relationships>();
            RelationshipsContainer myRelationshipContainer = this.provider.GetComponent<ResourceLocator>().Get<RelationshipsContainer>();
            Dictionary<string, TalkInteract> npcDict = new Dictionary<string, TalkInteract>();
            
            yield return new WaitUntil(() => targetSensor.detectedObjects.Count > 0);
            
            foreach (var npc in targetSensor.detectedObjects)
            {
                ComplexCharacterData characterData = npc.GetComponent<ResourceLocator>().Get<ComplexCharacterData>();
                agents.Add(characterData);
                relationships.Add(myRelationshipContainer.GetRelationship(characterData.characterName));
                npcDict[characterData.characterName] = npc;
            }
            
            var socialData = new SocialAgentData
            {
                agents = agents,
                relationships = relationships
            };
            
            yield return ServiceLocator.Global.Get<IPredictionService>().Predict(GOAPPromptProvider.Instance.talkPromptDefinition, socialData);
            
            if(string.IsNullOrEmpty(socialData.characterName) || !npcDict.ContainsKey(socialData.characterName))
            {
                Debug.LogWarning("No valid character selected for dialogue.");
                _coroutineDone = true;
                yield break;
            }
            
            targetSensor.SetTarget(npcDict[socialData.characterName]);
            _coroutineDone = true;
        }
    }
}