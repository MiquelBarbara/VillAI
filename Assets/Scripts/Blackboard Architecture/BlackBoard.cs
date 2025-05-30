using System;
using System.Collections.Generic;

namespace Blackboard_Architecture
{
    /// <summary>
    /// Represents a key in the blackboard system. The key is immutable and based on a hashed string.
    /// </summary>
    [Serializable]
    public readonly struct BlackboardKey : IEquatable<BlackboardKey>
    {
        private readonly string _name;
        private readonly int _hashedKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlackboardKey"/> struct with the specified name.
        /// </summary>
        /// <param name="name">The name of the key.</param>
        public BlackboardKey(string name)
        {
            this._name = name;
            _hashedKey = name.ComputeFNV1aHash();
        }

        /// <summary>
        /// Determines whether this instance is equal to another <see cref="BlackboardKey"/>.
        /// </summary>
        /// <param name="other">The other <see cref="BlackboardKey"/> to compare with.</param>
        /// <returns>true if equal; otherwise, false.</returns>
        public bool Equals(BlackboardKey other)
        {
            return _hashedKey == other._hashedKey;
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>true if equal; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return obj is BlackboardKey other && Equals(other);
        }

        /// <summary>
        /// Gets the hash code for the current <see cref="BlackboardKey"/>.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return _hashedKey;
        }

        /// <summary>
        /// Returns a string that represents the current <see cref="BlackboardKey"/>.
        /// </summary>
        /// <returns>The key name as a string.</returns>
        public override string ToString()
        {
            return _name;
        }

        /// <summary>
        /// Determines whether two specified <see cref="BlackboardKey"/> instances have the same value.
        /// </summary>
        /// <param name="lhs">The first <see cref="BlackboardKey"/>.</param>
        /// <param name="rhs">The second <see cref="BlackboardKey"/>.</param>
        /// <returns>true if both keys are equal; otherwise, false.</returns>
        public static bool operator ==(BlackboardKey lhs, BlackboardKey rhs)
        {
            return lhs._hashedKey == rhs._hashedKey;
        }

        /// <summary>
        /// Determines whether two specified <see cref="BlackboardKey"/> instances have different values.
        /// </summary>
        /// <param name="lhs">The first <see cref="BlackboardKey"/>.</param>
        /// <param name="rhs">The second <see cref="BlackboardKey"/>.</param>
        /// <returns>true if keys are not equal; otherwise, false.</returns>
        public static bool operator !=(BlackboardKey lhs, BlackboardKey rhs)
        {
            return !(lhs == rhs);
        }
    }

    /// <summary>
    /// Represents an entry in the blackboard, holding a key and its corresponding value.
    /// </summary>
    /// <typeparam name="T">The type of the value stored in the entry.</typeparam>
    [Serializable]
    public class BlackboardEntry<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlackboardEntry{T}"/> class.
        /// </summary>
        /// <param name="key">The key associated with the entry.</param>
        /// <param name="value">The value stored in the entry.</param>
        public BlackboardEntry(BlackboardKey key, T value)
        {
            Key = key;
            Value = value;
            ValueType = typeof(T);
        }

        /// <summary>
        /// Gets the key associated with this entry.
        /// </summary>
        public BlackboardKey Key { get; }

        /// <summary>
        /// Gets the value stored in this entry.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Gets the type of the value stored in this entry.
        /// </summary>
        public Type ValueType { get; }

        /// <summary>
        /// Determines whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>true if equal; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return obj is BlackboardEntry<T> other && other.Key == Key;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>The hash code for the entry.</returns>
        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }
    }

    /// <summary>
    /// Represents a blackboard for storing key-value pairs used in game systems.
    /// </summary>
    [Serializable]
    public class Blackboard
    {
        private Dictionary<BlackboardKey, object> _entries = new();
        private Dictionary<string, BlackboardKey> _keyRegistry = new();

        /// <summary>
        /// Gets the list of actions that have been passed.
        /// </summary>
        public List<Action> PassedActions { get; } = new();

        /// <summary>
        /// Adds an action to the list of passed actions.
        /// </summary>
        /// <param name="action">The action to add.</param>
        public void AddAction(Action action)
        {
            Preconditions.CheckNotNull(action);
            PassedActions.Add(action);
        }

        /// <summary>
        /// Clears all actions from the passed actions list.
        /// </summary>
        public void ClearActions()
        {
            PassedActions.Clear();
        }

        /// <summary>
        /// Logs the key and value of each entry in the blackboard for debugging purposes.
        /// </summary>
        public void Debug()
        {
            foreach (var entry in _entries)
            {
                var entryType = entry.Value.GetType();

                if (entryType.IsGenericType && entryType.GetGenericTypeDefinition() == typeof(BlackboardEntry<>))
                {
                    var valueProperty = entryType.GetProperty("Value");
                    if (valueProperty == null) continue;
                    var value = valueProperty.GetValue(entry.Value);
                }
            }
        }

        /// <summary>
        /// Attempts to get the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">The expected type of the value.</typeparam>
        /// <param name="key">The key to search for.</param>
        /// <param name="value">
        /// When this method returns, contains the value associated with the specified key, if found; otherwise, the default value.
        /// </param>
        /// <returns>true if the key was found; otherwise, false.</returns>
        public bool TryGetValue<T>(BlackboardKey key, out T value)
        {
            if (_entries.TryGetValue(key, out var entry) && entry is BlackboardEntry<T> castedEntry)
            {
                value = castedEntry.Value;
                return true;
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Sets the value for the specified key in the blackboard.
        /// </summary>
        /// <typeparam name="T">The type of the value to set.</typeparam>
        /// <param name="key">The key associated with the value.</param>
        /// <param name="value">The value to set.</param>
        public void SetValue<T>(BlackboardKey key, T value)
        {
            _entries[key] = new BlackboardEntry<T>(key, value);
        }

        /// <summary>
        /// Gets an existing key or registers a new key with the specified name.
        /// </summary>
        /// <param name="keyName">The name of the key.</param>
        /// <returns>The registered <see cref="BlackboardKey"/>.</returns>
        public BlackboardKey GetOrRegisterKey(string keyName)
        {
            Preconditions.CheckNotNull(keyName);

            if (!_keyRegistry.TryGetValue(keyName, out var key))
            {
                key = new BlackboardKey(keyName);
                _keyRegistry[keyName] = key;
            }

            return key;
        }

        /// <summary>
        /// Checks if the blackboard contains an entry for the specified key.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns>true if the key exists; otherwise, false.</returns>
        public bool ContainsKey(BlackboardKey key)
        {
            return _entries.ContainsKey(key);
        }

        /// <summary>
        /// Removes the entry associated with the specified key from the blackboard.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        public void Remove(BlackboardKey key)
        {
            _entries.Remove(key);
        }
    }
}
