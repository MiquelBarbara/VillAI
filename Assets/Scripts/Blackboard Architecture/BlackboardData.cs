using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace Blackboard_Architecture
{
    /// <summary>
    /// ScriptableObject containing data to initialize a Blackboard.
    /// </summary>
    [CreateAssetMenu(fileName = "New Blackboard Data", menuName = "Data/Blackboard Data")]
    public class BlackboardData : ScriptableObject
    {
        /// <summary>
        /// List of entries that will be set on the Blackboard.
        /// </summary>
        public List<BlackboardEntryData> entries = new();

        /// <summary>
        /// Sets the values from this data onto the specified Blackboard.
        /// </summary>
        /// <param name="blackboard">The Blackboard to update.</param>
        public void SetValuesOnBlackboard(Blackboard blackboard)
        {
            foreach (var entry in entries)
                entry.SetValueOnBlackboard(blackboard);
        }
    }

    /// <summary>
    /// Represents a serializable data entry for the Blackboard.
    /// </summary>
    [Serializable]
    public class BlackboardEntryData : ISerializationCallbackReceiver
    {
        // Dispatch table to set different types of value on the blackboard
        private static readonly Dictionary<AnyValue.ValueType, Action<Blackboard, BlackboardKey, AnyValue>>
            SetValueDispatchTable = new()
            {
                { AnyValue.ValueType.Int, (blackboard, key, anyValue) => blackboard.SetValue(key, anyValue.intValue) },
                { AnyValue.ValueType.Float, (blackboard, key, anyValue) => blackboard.SetValue(key, anyValue.floatValue) },
                { AnyValue.ValueType.Bool, (blackboard, key, anyValue) => blackboard.SetValue(key, anyValue.boolValue) },
                { AnyValue.ValueType.String, (blackboard, key, anyValue) => blackboard.SetValue(key, anyValue.stringValue) },
                { AnyValue.ValueType.Vector3, (blackboard, key, anyValue) => blackboard.SetValue(key, anyValue.vector3Value) },
                { AnyValue.ValueType.GameObject, (blackboard, key, anyValue) => blackboard.SetValue(key, anyValue.gameObjectValue) },
                { AnyValue.ValueType.Transform, (blackboard, key, anyValue) => blackboard.SetValue(key, anyValue.transformValue) },
                { AnyValue.ValueType.Behavior, (blackboard, key, anyValue) => blackboard.SetValue(key, anyValue.behaviorValue) },
                { AnyValue.ValueType.Enum, (blackboard, key, anyValue) => blackboard.SetValue(key, anyValue.enumValue) },
                { AnyValue.ValueType.Event, (blackboard, key, anyValue) => blackboard.SetValue(key, anyValue.eventValue) },
                { AnyValue.ValueType.List, (blackboard, key, anyValue) => blackboard.SetValue(key, anyValue.listValue) },
                { AnyValue.ValueType.Resource, (blackboard, key, anyValue) => blackboard.SetValue(key, anyValue.resourceValue) },
                { AnyValue.ValueType.Dictionary, (blackboard, key, anyValue) => blackboard.SetValue(key, anyValue.dictionaryValue) }
            };

        /// <summary>
        /// The key name associated with this entry.
        /// </summary>
        public string keyName;

        /// <summary>
        /// The type of the value stored.
        /// </summary>
        public AnyValue.ValueType valueType;

        /// <summary>
        /// The value data.
        /// </summary>
        public AnyValue value;

        /// <summary>
        /// Callback invoked before serialization. (No implementation needed.)
        /// </summary>
        public void OnBeforeSerialize()
        {
        }

        /// <summary>
        /// Callback invoked after deserialization. Sets the type of the value.
        /// </summary>
        public void OnAfterDeserialize()
        {
            value.type = valueType;
        }

        /// <summary>
        /// Sets this entry's value on the specified Blackboard.
        /// </summary>
        /// <param name="blackboard">The Blackboard to update.</param>
        public void SetValueOnBlackboard(Blackboard blackboard)
        {
            var key = blackboard.GetOrRegisterKey(keyName);
            SetValueDispatchTable[value.type](blackboard, key, value);
        }
    }

    /// <summary>
    /// Represents a flexible value type that can hold various data types.
    /// </summary>
    [Serializable]
    public struct AnyValue
    {
        /// <summary>
        /// Enumerates the supported types for AnyValue.
        /// </summary>
        public enum ValueType
        {
            Int,
            Float,
            Bool,
            String,
            Vector3,
            GameObject,
            Transform,
            Behavior,
            Enum,
            Event,
            List,
            Resource,
            Dictionary
        }

        /// <summary>
        /// Gets or sets the type of this value.
        /// </summary>
        public ValueType type;

        /// <summary>
        /// Integer value.
        /// </summary>
        public int intValue;

        /// <summary>
        /// Float value.
        /// </summary>
        public float floatValue;

        /// <summary>
        /// Boolean value.
        /// </summary>
        public bool boolValue;

        /// <summary>
        /// String value.
        /// </summary>
        public string stringValue;

        /// <summary>
        /// Vector3 value.
        /// </summary>
        public Vector3 vector3Value;

        /// <summary>
        /// GameObject value.
        /// </summary>
        public GameObject gameObjectValue;

        /// <summary>
        /// Transform value.
        /// </summary>
        public Transform transformValue;

        /// <summary>
        /// MonoBehaviour value.
        /// </summary>
        public MonoBehaviour behaviorValue;

        /// <summary>
        /// Enum value represented as a string.
        /// </summary>
        public string enumValue; // Serialized as string for flexibility

        /// <summary>
        /// UnityEvent value.
        /// </summary>
        public UnityEvent eventValue;

        /// <summary>
        /// Nested list of AnyValue.
        /// </summary>
        public List<AnyValue> listValue;

        /// <summary>
        /// Resource value.
        /// </summary>
        public Object resourceValue;
        
        public Dictionary<string,object> dictionaryValue;

        /// <summary>
        /// Implicit conversion to int.
        /// </summary>
        /// <param name="value">The AnyValue instance.</param>
        public static implicit operator int(AnyValue value)
        {
            return value.type == ValueType.Int ? value.intValue : default;
        }

        /// <summary>
        /// Implicit conversion to float.
        /// </summary>
        /// <param name="value">The AnyValue instance.</param>
        public static implicit operator float(AnyValue value)
        {
            return value.type == ValueType.Float ? value.floatValue : default;
        }

        /// <summary>
        /// Implicit conversion to bool.
        /// </summary>
        /// <param name="value">The AnyValue instance.</param>
        public static implicit operator bool(AnyValue value)
        {
            return value.type == ValueType.Bool ? value.boolValue : default;
        }

        /// <summary>
        /// Implicit conversion to string.
        /// </summary>
        /// <param name="value">The AnyValue instance.</param>
        public static implicit operator string(AnyValue value)
        {
            return value.type == ValueType.String ? value.stringValue : default;
        }

        /// <summary>
        /// Implicit conversion to Vector3.
        /// </summary>
        /// <param name="value">The AnyValue instance.</param>
        public static implicit operator Vector3(AnyValue value)
        {
            return value.type == ValueType.Vector3 ? value.vector3Value : default;
        }

        /// <summary>
        /// Implicit conversion to GameObject.
        /// </summary>
        /// <param name="value">The AnyValue instance.</param>
        public static implicit operator GameObject(AnyValue value)
        {
            return value.type == ValueType.GameObject ? value.gameObjectValue : default;
        }

        /// <summary>
        /// Implicit conversion to Transform.
        /// </summary>
        /// <param name="value">The AnyValue instance.</param>
        public static implicit operator Transform(AnyValue value)
        {
            return value.type == ValueType.Transform ? value.transformValue : default;
        }

        /// <summary>
        /// Implicit conversion to MonoBehaviour.
        /// </summary>
        /// <param name="value">The AnyValue instance.</param>
        public static implicit operator MonoBehaviour(AnyValue value)
        {
            return value.type == ValueType.Behavior ? value.behaviorValue : default;
        }

        public static implicit operator Dictionary<string, object>(AnyValue value)
        {
            if (value.type != ValueType.Dictionary || value.listValue == null) return default;
            var dict = new Dictionary<string, object>();
            foreach (var item in value.listValue.Where(
                         item => item.type == ValueType.String && item.stringValue != null))
            {
                dict[item.stringValue] = item;
            }

            return dict;
        }
    }
}
