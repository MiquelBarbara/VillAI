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
        
        [Tooltip("Unique identifier; if left empty, a GUID will be auto-generated")]
        public string id = Guid.NewGuid().ToString();

        [Tooltip("UTC ISO-8601 timestamp; filled automatically on instantiation if left empty")]
        public string timestamp = DateTime.UtcNow.ToString("o");

        /// <summary>
        /// Generic field for any extra metadata (tags, hashes, etc.).
        /// </summary>
        public Dictionary<string, string> metadata = new();
    }
}