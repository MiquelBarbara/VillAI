using UnityEngine;
using System;
using System.Reflection;
using Utilities.EnumExtensions;

namespace LLM.Tools
{
    [CreateAssetMenu(fileName = "NewToolDescriptor", menuName = "LLM/Tool Descriptor")]
    [Serializable]
    public class ToolDefinition : EnumScriptableObject
    {
        [Tooltip("Unique identifier matching ToolMethod attribute")]
        public string toolName;

        [TextArea(2, 4)]
        public string description;
        
        [HideInInspector]public string methodClassName;
        [HideInInspector]public string methodName;
        [HideInInspector] public string[] parameterTypes;
        
        [NonSerialized] private Type cachedType;
        [NonSerialized] private MethodInfo cachedMethod;

        public void CacheMethodInfo()
        {
            if (cachedMethod != null) return;
            cachedType = Type.GetType(methodClassName);
            if (cachedType != null)
            {
                cachedMethod = cachedType.GetMethod(methodName, 
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            }
        }

        public MethodInfo GetCachedMethod()
        {
            CacheMethodInfo();
            return cachedMethod;
        }
        public Type GetCachedType() => cachedType;
        
    }
}