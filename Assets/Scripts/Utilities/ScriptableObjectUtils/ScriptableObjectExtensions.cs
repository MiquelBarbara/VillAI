using UnityEngine;

namespace Utilities.ScriptableObjectUtils
{
    /// <summary>
    /// Utility class for cloning ScriptableObjects.
    /// </summary>
    public class ScriptableObjectExtensions
    {
        public static T Clone<T>(T original) where T : ScriptableObject
        {
            T clone = ScriptableObject.CreateInstance<T>();
            foreach (var field in typeof(T).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance))
            {
                field.SetValue(clone, field.GetValue(original));
            }
            return clone;
        }
    }
}