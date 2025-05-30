using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

public class IgnoreUnityTypesResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(
        MemberInfo member, 
        MemberSerialization memberSerialization
    )
    {
        JsonProperty property = base.CreateProperty(member, memberSerialization);

        // Skip Unity-specific properties (e.g., "name", "hideFlags")
        if (member.DeclaringType == typeof(UnityEngine.Object) || 
            member.Name == "name" || 
            member.Name == "hideFlags")
        {
            property.Ignored = true;
        }

        // Force serialization of private fields (even without [SerializeField])
        property.Writable = true;
        property.Readable = true;
        return property;
    }

    protected override JsonContract CreateContract(Type objectType)
    {
        // Skip all Unity objects except ScriptableObject-derived types
        if (typeof(UnityEngine.Object).IsAssignableFrom(objectType) &&
            !typeof(ScriptableObject).IsAssignableFrom(objectType))
        {
            return CreateObjectContract(objectType); // Skip non-ScriptableObject Unity types
        }

        return base.CreateContract(objectType);
    }
}