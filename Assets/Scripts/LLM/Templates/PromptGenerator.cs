using System;
using System.Text; using System.Collections.Generic;
using LLM.Mcp;
using LLM.Services;
using LLM.Templates;
using LLM.Tools;
using UnityEngine;
using Utilities.ScriptableObjectExtensions;
using TextAsset = UnityEngine.TextCore.Text.TextAsset;

namespace LLM.Utilities
{
    /// <summary>
    ///     Utility class that generates the final prompt from a template definition and input data.
    /// </summary>

    public class PromptGenerator
    {
        
        public PromptGenerator()
        {

        }
        
        public List<McpMessage> GenerateMessages(PromptDefinition template, DataTransferObject data)
        {
            var messages = new List<McpMessage>();
            
            // 2) system (si existe)
            if (!string.IsNullOrWhiteSpace(template.systemPrompt))
                messages.Add(BuildMessage(McpMessage.McpRole.system, template.systemPrompt));

            // 3) developer (si existe)
            if (!string.IsNullOrWhiteSpace(template.developerPrompt))
                messages.Add(BuildMessage(McpMessage.McpRole.developer, template.developerPrompt));

            // 4) user  (siempre)
            messages.Add(BuildMessage(McpMessage.McpRole.user, template.templateText, data));

            return messages;
        }
        
        private McpMessage BuildMessage(McpMessage.McpRole role, string rawText, DataTransferObject data)
        {
            string content = ReplacePlaceholders(rawText, data);
            return new McpMessage { role = role, content = content };
        }
        
        private McpMessage BuildMessage(McpMessage.McpRole role, string rawText)
        {
            return new McpMessage { role = role, content = rawText };
        }
        
        public List<ToolDefinition> GetToolDescriptors(PromptDefinition templateData)
        {
            return templateData.toolDescriptors;
        }
        public List<ResourceType> GetResources(PromptDefinition templateData)
        {
            var resources = new List<ResourceType>();
            

            return resources;
        }

        private string ReplacePlaceholders(string text, DataTransferObject data)
        {
            text = ReplaceInputs(text, data);
            text = ReplaceOutputs(text, data);
            return text;
        }
        
        private string ReplaceInputs<TData>(string text, TData data) where TData : class
        {
            var inputs = ReflectionExtensions.GetObjectByAttribute(data, typeof(InputAttribute));
            foreach (var kvp in inputs)
            {
                var token = $"{{{kvp.Key}}}";
                if (!text.Contains(token)) continue;
                text = text.Replace(token, JsonParser.ToJson(kvp.Value));
            }
            return text;
        }
        
        private string ReplaceOutputs<TData>(string text, TData data) where TData : class
        {
            var outputs = ReflectionExtensions.GetTypesByAttribute(data, typeof(OutputAttribute));
            foreach (var kvp in outputs)
            {
                var token = $"{{{kvp.Key}}}";
                if (!text.Contains(token)) continue;
                text = text.Replace(token, $"{kvp.Key} (type ->" + kvp.Value + ")");
            }

            return text;
        }
    }
}