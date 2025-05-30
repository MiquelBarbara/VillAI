using System.Collections.Generic;
using UnityEngine;
using Utilities.ScriptableObjectExtensions;

namespace Systems.SaveSystem.Memory
{
    
    [CreateAssetMenu(fileName = "KnowledgeContainer", menuName = "Resources/KnowledgeContainer")]
    public class KnowledgeContainer: Summarizable<string> { }
}