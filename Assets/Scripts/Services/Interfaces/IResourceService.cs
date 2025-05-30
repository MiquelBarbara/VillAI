using Systems.SaveSystem.Memory.Conversations;
using Utilities.ScriptableObjectExtensions;

public interface IResourceService
{
    void RegisterSource(string npcId, ResourceLocator memoryComponent);
    TMemory GetResource<TMemory>(string npcId) where TMemory : ScriptableSave;
}