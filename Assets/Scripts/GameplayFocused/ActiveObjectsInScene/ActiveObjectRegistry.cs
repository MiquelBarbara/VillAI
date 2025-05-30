using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic registry for active objects of type T.
/// Sensors can query ActiveObjectRegistry<T>.ActiveObjects to find all currently enabled instances.
/// </summary>
public static class ActiveObjectRegistry<T> where T : Component
{
    private static readonly List<T> _active = new List<T>();
    public static IReadOnlyList<T> ActiveObjects => _active;

    public static void Register(T obj)
    {
        if (obj != null && !_active.Contains(obj))
            _active.Add(obj);
    }

    public static void Unregister(T obj)
    {
        if (obj != null)
            _active.Remove(obj);
    }
}