using System;
using System.Collections;
using System.Linq;
using LLM.Services;
using LLM.Templates;
using NPCs;
using Systems.SaveSystem.Memory;
using UnityServiceLocator;

namespace Utilities.ScriptableObjectExtensions
{
    using UnityEngine;
    using System.Collections.Generic;

    public class SummarizeData<T>: DataTransferObject
    {
        [Input] public List<T> information;
        [Input] public string actualSummary;
        
        [Output] public string summary;
    }
    
    public class ResourceLocator : MonoBehaviour
    {
        
        [SerializeField] PromptDefinition summaryPromptDefinition;
        
        [SerializeField] string Key;
        [SerializeField] private List<ScriptableObject> _resources;
    
        private readonly Dictionary<System.Type, ScriptableObject> _resourceDictionary = 
            new Dictionary<System.Type, ScriptableObject>();

        private void Awake()
        {
            if (_resources != null)
            {
                foreach (var resource in _resources)
                {
                    if (resource == null)
                    {
                        Debug.LogWarning("Found a null resource in the list.", this);
                        continue;
                    }
                    RegisterResource(resource);
                }
            }
            else
            {
                Debug.LogWarning("Resources list is null.", this);
            }
        }

        public void Start()
        {
            ServiceLocator.Global.Get<IResourceService>().RegisterSource(Get<ComplexCharacterData>().characterName, this);
        }

        public T Get<T>() where T : ScriptableObject
        {
            var type = typeof(T);
            if (_resourceDictionary.TryGetValue(type, out ScriptableObject resource))
            {
                return (T)resource;
            }
            return null;
        }
        
        public void RegisterResource<T>(T resource) where T : ScriptableObject
        {
            var type = resource.GetType();
            if (_resourceDictionary.ContainsKey(type))
            {
                Debug.LogWarning($"Overwriting existing resource of type {type.Name}", this);
            }
            _resourceDictionary[type] = resource;
            
            if(resource is ISummarizable summarizable)
            {
                summarizable.ResourceLocatorSubscribe(this);
            }
        }
        
        public void HandleResourceSummary<T>(Summarizable<T> resource)
        {
            if (resource == null) return;
            StartCoroutine(SummarizeResource(resource));
        }
        
        public List<ScriptableObject> GetAllResources()
        {
            return new List<ScriptableObject>(_resourceDictionary.Values);
        }

        private IEnumerator SummarizeResource<T>(Summarizable<T> resource)
        {
            var data = new SummarizeData<T>()
            {
                information = resource.entries,
                actualSummary = resource.GetSummary()
            };

            yield return ServiceLocator.Global.Get<IPredictionService>().Predict(summaryPromptDefinition, data);
            
            if (data.summary != null)
            {
                resource.SetSummary(data.summary);
            }
        }
    }
}