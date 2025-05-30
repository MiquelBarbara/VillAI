namespace Utilities.EnumExtensions
{
    // EnumScriptableObject.cs
    using UnityEngine;

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