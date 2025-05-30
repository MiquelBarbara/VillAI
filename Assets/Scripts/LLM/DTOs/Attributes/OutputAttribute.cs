using System;

namespace LLM.Templates
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class OutputAttribute : Attribute { }
}