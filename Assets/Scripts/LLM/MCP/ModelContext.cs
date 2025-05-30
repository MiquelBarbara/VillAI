using System.Collections.Generic;
using LLM.Templates;
using LLM.Tools;
using UnityEngine;

namespace LLM.Mcp
{
    /// <summary>
    /// Container of the complete context that will travel to the MCP server.
    /// </summary>
    [System.Serializable]
    public class ModelContext
    {
        public string conversationId; 
        public List<McpMessage> messages = new();
        public List<ToolDefinition> tools = new();
        public List<ScriptableObject> resources = new();
        public ModelConfig modelConfig;
        public string baseHash;
        public bool isDiff;
    }
}