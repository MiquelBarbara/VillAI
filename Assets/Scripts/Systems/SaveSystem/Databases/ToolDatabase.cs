using LLM.Tools;
using UnityEngine;

namespace Systems.SaveSystem.Databases
{
    [CreateAssetMenu (fileName = "NewToolDatabase", menuName = "Databases/Tool Database")]
    public class ToolDatabase: GenericDatabase<ToolDefinition>
    {
        
    }
}