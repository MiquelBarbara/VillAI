using System.Collections.Generic;
using LLM.Tools;
using Systems.SaveSystem.Memory;
using Systems.SaveSystem.Memory.Conversations;
using UnityEngine;
using Utilities.ScriptableObjectExtensions;

/// <summary>
/// ResourceService is responsible for managing resources associated with NPCs.
/// </summary>
public class ResourceService : IResourceService
{
    private readonly Dictionary<string, ResourceLocator> _memoryMap = new();
    
    /// <summary>
    /// Registers a source of memory for an NPC identified by npcId.
    /// </summary>
    /// <param name="npcId"> The unique identifier for the NPC.</param>
    /// <param name="memoryComponent"> The ResourceLocator component that holds the memory data.</param>
    public void RegisterSource(string npcId, ResourceLocator memoryComponent)
    {
        _memoryMap[npcId] = memoryComponent;
    }

    /// <summary>
    /// Retrieves a specific type of memory resource for an NPC identified by npcId.
    /// </summary>
    /// <param name="npcId"> The unique identifier for the NPC.</param>
    /// <typeparam name="TMemory"> The type of memory resource to retrieve, which must inherit from ScriptableSave.</typeparam>
    /// <returns> Returns the requested memory resource if found, otherwise null.</returns>
    public TMemory GetResource<TMemory>(string npcId) where TMemory : ScriptableSave
    {
        return _memoryMap.TryGetValue(npcId, out var comp) ? comp.Get<TMemory>() : null;
    }
    
}