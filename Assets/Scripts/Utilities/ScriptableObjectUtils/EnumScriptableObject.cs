namespace Utilities.EnumExtensions
{
    using UnityEngine;

    /// <summary>
    /// Base class for ScriptableObjects that represent enumerations.
    /// </summary>
    public abstract class EnumScriptableObject : ScriptableObject
    {
        public override string ToString() => name;
        
        public string DisplayName
        {
            get => name;
            set => name = value;
        }
    }
}