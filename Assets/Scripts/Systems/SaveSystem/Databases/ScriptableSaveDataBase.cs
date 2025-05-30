using UnityEngine;
using Utilities.ScriptableObjectExtensions;

namespace Systems.SaveSystem.Databases
{
    [CreateAssetMenu(fileName = "ScriptableSaveDataBase", menuName = "Databases/ScriptableSaveDataBase")]
    public class ScriptableSaveDataBase : GenericDatabase<ScriptableSave>
    {
    
    }
}