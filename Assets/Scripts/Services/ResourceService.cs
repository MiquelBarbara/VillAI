using System.Collections.Generic;
using LLM.Tools;
using Systems.SaveSystem.Memory;
using Systems.SaveSystem.Memory.Conversations;
using UnityEngine;
using Utilities.ScriptableObjectExtensions;

public class ResourceService : IResourceService
{
    private readonly Dictionary<string, ResourceLocator> _memoryMap = new();
    
    public void RegisterSource(string npcId, ResourceLocator memoryComponent)
    {
        _memoryMap[npcId] = memoryComponent;
    }

    public TMemory GetResource<TMemory>(string npcId) where TMemory : ScriptableSave
    {
        return _memoryMap.TryGetValue(npcId, out var comp) ? comp.Get<TMemory>() : null;
    }
    
}