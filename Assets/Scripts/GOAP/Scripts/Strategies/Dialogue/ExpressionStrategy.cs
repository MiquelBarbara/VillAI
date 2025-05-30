using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EntitiesRelated.Core;
using GOAP.Scripts;
using GOAP.Scripts.AgentStats;
using LLM.Services;
using LLM.Templates;
using NPCs;
using Systems.SaveSystem.Memory;
using UI;
using UnityEngine;
using UnityServiceLocator;
using Utilities.ScriptableObjectExtensions;

public class ExpressData : DataTransferObject
{
    [Input] public List<string> reactableObject;
    [Input] public ComplexCharacterData character;
    [Input] public Thoughts thoughts;
    
    [Output] public string message;
}


public class ExpressionStrategy : CoroutineStrategy
{

    private List<ReactableObject> reactableObjects;

    public ExpressionStrategy(MonoBehaviour provider, List<ReactableObject> reactableObjects) : base(provider)
    {
        this.provider = provider;
        this.reactableObjects = reactableObjects;
    }
    
    protected override IEnumerator Predict()
    {
        
        var expressData = new ExpressData()
        {
            reactableObject = reactableObjects.Select(reactable => reactable.GetDescription()).ToList(),
            thoughts = provider.GetComponent<ResourceLocator>().Get<Thoughts>(),
            character = provider.GetComponent<ResourceLocator>().Get<ComplexCharacterData>()
        };
        
        ServiceLocator.Global.Get<IPredictionService>(out var service);
        yield return service.Predict(GOAPPromptProvider.Instance.expressPromptDefinition, expressData);
        
        if (expressData.message == null || expressData.message.Trim().Length == 0)
        {
            Debug.LogWarning("Expression message is empty or null, skipping expression.");
            _coroutineDone = true;
            yield break;
        }
        
        yield return UIManager.Instance.ShowBubble(provider.transform, StringExtensions.SplitStringIntoSentences(expressData.message), 5f);
        provider.GetComponent<StatSystem>().IncreaseStat("Relive", 100);
        _coroutineDone = true;
    }
    
    public bool CanPerform => !Complete;
    public bool Complete => _coroutineDone;
}