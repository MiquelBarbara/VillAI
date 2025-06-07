using System;
using System.Collections.Generic;
using System.Reflection;
using LLM.Services;
using UnityEngine;

namespace LLM.Templates
{
    /// <summary>
    /// Represents a base class for data transfer objects (DTOs) used in the LLM system.
    /// </summary>
    public abstract class DataTransferObject
    {
        /// <summary>
        /// Updates the fields of the DTO with the provided outputs.
        /// </summary>
        /// <param name="outputs"> A dictionary containing field names and their corresponding values to update.</param>
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