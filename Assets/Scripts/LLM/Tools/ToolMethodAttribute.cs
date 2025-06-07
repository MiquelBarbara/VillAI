namespace LLM.Tools
{
    using System;

    /// <summary>
    /// Attribute to mark methods as tool methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class ToolMethodAttribute : Attribute
    {
        public string ToolName { get; }
        public string Description { get; }

        public ToolMethodAttribute(string toolName, string description)
        {
            ToolName = toolName;
            Description = description;
        }
    }
}