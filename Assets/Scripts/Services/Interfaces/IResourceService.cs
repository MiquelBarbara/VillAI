using Systems.SaveSystem.Memory.Conversations;
using Utilities.ScriptableObjectExtensions;

/// <summary>
/// IResourceService is responsible for managing resources associated with NPCs.
/// </summary>
public interface IResourceService
{
    void RegisterSource(string npcId, ResourceLocator memoryComponent);
    TMemory GetResource<TMemory>(string npcId) where TMemory : ScriptableSave;
}