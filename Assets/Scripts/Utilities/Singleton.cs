using UnityEngine;


/// <summary>
/// A generic singleton class for Unity components.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : Component
{
    protected static T instance;
    public static bool HasInstance => instance != null;
    public static T Current => instance;

    public static T Instance
    {
        get
        {
            if (instance != null) return instance;
            // Try to find an existing instance in the scene
            instance = FindObjectOfType<T>();
            if (instance != null) return instance;
            // If no instance exists, create a new GameObject and add the component
            var obj = new GameObject();
            obj.name = typeof(T).Name + "AutoCreates";
            instance = obj.AddComponent<T>();

            return instance;
        }
    }

    protected virtual void Awake()
    {
        InitializeSingleton();
    }

    public static T TryGetInstance()
    {
        return HasInstance ? instance : null;
    }

    /// <summary>
    /// Initializes the singleton instance.
    /// </summary>
    protected virtual void InitializeSingleton()
    {
        if (!Application.isPlaying) return;

        instance = this as T;
    }
}