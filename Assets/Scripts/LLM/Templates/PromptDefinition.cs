using System;
using System.Collections.Generic;
using UnityEngine;
using LLM.Tools;

namespace LLM.Templates
{
    /// <summary>
    /// Represents a text template with input and output placeholders.
    /// </summary>
    [CreateAssetMenu(fileName = "NewPromptDefinition", menuName = "LLM/Prediction Definition")]
    public class PromptDefinition : ScriptableObject
    {
        [TextArea(4, 8)]
        [Tooltip("Write the template text, using placeholders like {Agent}, {Target}, etc.")]
        public string templateText;
        
        [TextArea(2, 6)]
        [Tooltip("Prompt sent as 'system' role. Optional.")]
        public string systemPrompt;

        [TextArea(2, 6)]
        [Tooltip("Prompt sent as 'developer' role. Optional.")]
        public string developerPrompt;
        
        public List<ToolDefinition> toolDescriptors;
        public List<ResourceType> resources;
        
        public ModelConfig modelConfig;
    }
}
