using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace LLM.Utilities
{
    public static class JsonParser
    {
        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Converters = { new StringEnumConverter() },
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        /// <summary>
        /// Deserializes JSON to the specified .NET type
        /// </summary>
        public static T FromJson<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json, _settings);
            }
            catch (JsonException ex)
            {
                Debug.LogError($"JSON deserialization failed: {ex.Message}\nJSON: {json}");
                return default;
            }
        }

        /// <summary>
        /// Serializes the specified object to a JSON string
        /// </summary>
        public static string ToJson(object obj, bool prettyPrint = false)
        {
            try
            {
                return JsonConvert.SerializeObject(obj, prettyPrint ? Formatting.Indented : Formatting.None, _settings);
            }
            catch (JsonException ex)
            {
                Debug.LogError($"JSON serialization failed: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Converts a JSON string to a Dictionary<string, object>
        /// </summary>
        public static Dictionary<string, object> ToDictionary(string json)
        {
            try
            {
                // Parse the JSON into a JToken
                var token = JToken.Parse(json);

                // Recursively convert the JToken into the appropriate C# type
                var _object = ConvertTokenToCSharpType(token);
                if (_object is Dictionary<string, object> data) return data;

                return null;
            }
            catch (JsonReaderException e)
            {
                Debug.Log($"Invalid JSON: {e.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// Merges two JSON strings into one
        /// </summary>
        public static string MergeJson(string json1, string json2)
        {
            try
            {
                var obj1 = JObject.Parse(json1);
                var obj2 = JObject.Parse(json2);
                obj1.Merge(obj2, new JsonMergeSettings
                {
                    MergeArrayHandling = MergeArrayHandling.Union
                });
                return obj1.ToString();
            }
            catch (JsonException ex)
            {
                Debug.LogError($"JSON merge failed: {ex.Message}");
                return null;
            }
        }
        
        private static object ConvertTokenToCSharpType(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    return ConvertToDictionary(token as JObject);
                case JTokenType.Array:
                    return ConvertToList(token as JArray);
                case JTokenType.Integer:
                    return token.ToObject<int>();
                case JTokenType.Float:
                    return token.ToObject<float>();
                case JTokenType.String:
                    return token.ToObject<string>();
                case JTokenType.Boolean:
                    return token.ToObject<bool>();
                case JTokenType.Null:
                    return null;
                default:
                    // Handle other types if needed
                    return token.ToString();
            }
        }
        
        private static Dictionary<string, object> ConvertToDictionary(JObject obj)
        {
            var dictionary = new Dictionary<string, object>();

            foreach (var property in obj.Properties())
                dictionary[property.Name] = ConvertTokenToCSharpType(property.Value);

            return dictionary;
        }

        /// <summary>
        ///     Converts a JArray to a List<object>.
        /// </summary>
        /// <param name="array">The JArray to convert.</param>
        /// <returns>A List<object> representing the JArray.</returns>
        private static List<object> ConvertToList(JArray array)
        {
            return array.Select(ConvertTokenToCSharpType).ToList();
        }
    }
}