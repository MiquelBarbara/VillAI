using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

public class ScriptableObjectConverter : JsonConverter<ScriptableObject>
{
    public override void WriteJson(JsonWriter writer, ScriptableObject value, JsonSerializer serializer)
    {
        // Serialize only the data fields, not the Unity object metadata
        var contract = (JsonObjectContract)serializer.ContractResolver.ResolveContract(value.GetType());
        writer.WriteStartObject();
        foreach (var property in contract.Properties)
        {
            if (property.Ignored) continue;
            if (property.PropertyType != null &&
                property.PropertyType.IsSubclassOf(typeof(UnityEngine.Object)) && 
                !typeof(ScriptableObject).IsAssignableFrom(property.PropertyType))
                continue;

            var memberValue = property.ValueProvider.GetValue(value);
            writer.WritePropertyName(property.PropertyName);
            serializer.Serialize(writer, memberValue);
        }
        writer.WriteEndObject();
    }

    public override ScriptableObject ReadJson(JsonReader reader, Type objectType, ScriptableObject existingValue, 
        bool hasExistingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException("Deserialization is not implemented here.");
    }
}