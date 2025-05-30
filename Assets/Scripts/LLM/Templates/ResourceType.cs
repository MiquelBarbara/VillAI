using UnityEngine;
using UnityEngine.Serialization;
using Utilities.EnumExtensions;

namespace LLM.Templates
{
    
    [CreateAssetMenu(fileName = "ResourceType", menuName = "LLM/ResourceType")]
    public class ResourceType: EnumScriptableObject
    {
        public string resourceName;
        public string resourceDescription;
        public string path;
    }
}