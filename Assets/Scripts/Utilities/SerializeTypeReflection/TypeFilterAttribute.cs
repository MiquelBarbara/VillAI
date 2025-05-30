using System;
using UnityEngine;

namespace Rleflection
{
    public class TypeFilterAttribute : PropertyAttribute
    {
        public TypeFilterAttribute(Type filterType)
        {
            Filter = type => !type.IsAbstract && !type.IsInterface && !type.IsGenericType &&
                             type.InheritsOrImplements(filterType);
        }

        public Func<Type, bool> Filter { get; }
    }
}