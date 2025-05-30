using System;
using UnityEditor;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
public class AllowAsParameterAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class TypeSelectorAttribute : PropertyAttribute
{
    public bool AllowLists { get; private set; }

    public TypeSelectorAttribute(bool allowLists = true)
    {
        AllowLists = allowLists;
    }
}

