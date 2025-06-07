using System;
using System.Collections.Generic;
using UnityEngine;

namespace LLM.Mcp
{
    /// <summary>
    /// Minimum context unit that will be sent to the LLM using the Model Context Protocol.
    /// </summary>
    [Serializable]
    public class McpMessage
    {
        public enum McpRole
        {
            system,
            developer,
            user,
            assistant,
            tool
        }

        [Tooltip("Role of the sender within the protocol (system, user, ...)")]
        public McpRole role;

        [TextArea(2,6)]
        [Tooltip("Text content or JSON (serialized as string)")]
        public string content;
    }
}