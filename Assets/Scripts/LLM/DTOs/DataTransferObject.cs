using System;
using System.Collections.Generic;
using System.Reflection;
using LLM.Services;
using UnityEngine;

namespace LLM.Templates
{
    public abstract class DataTransferObject
    {
        
        public void UpdateOutputs(Dictionary<string, object> outputs)
        {
            var type = GetType();
            foreach (var kvp in outputs)
            {
                var field = type.GetField(kvp.Key, BindingFlags.Public | BindingFlags.Instance);
                if (field == null || field.IsInitOnly) continue;
                try
                {
                    var convertedValue = field.FieldType.Cast(kvp.Value);
                    field.SetValue(this, convertedValue);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating field '{kvp.Key}': {ex.Message}");
                }
            }
        }
    }
}