// ToolDispatcher.cs
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using LLM.Tools;
using Systems.SaveSystem.Databases;

/// <summary>
/// ToolDispatcher is responsible for managing and invoking tools defined in the ToolDatabase.
/// </summary>
public class ToolDispatcher : MonoBehaviour
{
    [SerializeField] private ToolDatabase toolDatabase;
    public static ToolDispatcher Instance { get; private set; }

    private Dictionary<string, (MethodInfo method, Type type)> toolCache = 
        new Dictionary<string, (MethodInfo, Type)>();
        
    private Dictionary<Type, MonoBehaviour> instanceCache = 
        new Dictionary<Type, MonoBehaviour>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        BuildCache();
        DontDestroyOnLoad(gameObject);
    }

    private void BuildCache()
    {
        foreach (var tool in toolDatabase.Items)
        {
            tool.CacheMethodInfo();
            var method = tool.GetCachedMethod();
            var type = tool.GetCachedType();
            
            if (method != null && type != null)
            {
                toolCache[tool.toolName] = (method, type);
            }
        }
    }

    /// <summary>
    /// Attempts to invoke a tool method by its name with the provided inputs.
    /// </summary>
    /// <param name="toolName"> Name of the tool to invoke.</param>
    /// <param name="inputs"> Dictionary of input parameters for the tool method.</param>
    /// <param name="result"> Output parameter to hold the result of the method invocation.</param>
    /// <returns> True if the tool was successfully invoked, false otherwise.</returns>
    /// <exception cref="ArgumentException"> Thrown when a required parameter is missing and has no default value.</exception>
    public bool TryInvokeTool(string toolName, Dictionary<string, object> inputs, out object result)
    {
        result = null;
        if (!toolCache.TryGetValue(toolName, out var methodData))
        {
            Debug.LogError($"Tool not cached: {toolName}");
            return false;
        }

        try
        {
            if (!instanceCache.TryGetValue(methodData.type, out var instance))
            {
                instance = FindObjectOfType(methodData.type) as MonoBehaviour;
                if (instance == null)
                {
                    Debug.LogError($"No instance found for {methodData.type.Name}");
                    return false;
                }
                instanceCache[methodData.type] = instance;
            }

            ParameterInfo[] parameters = methodData.method.GetParameters();
            object[] methodArgs = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                if (!inputs.TryGetValue(parameters[i].Name, out object strValue))
                {
                    if (parameters[i].HasDefaultValue)
                        methodArgs[i] = parameters[i].DefaultValue;
                    else
                        throw new ArgumentException($"Missing parameter: {parameters[i].Name}");
                }
                else
                {
                    methodArgs[i] = Convert.ChangeType(strValue, parameters[i].ParameterType);
                }
            }

            result = methodData.method.Invoke(instance, methodArgs);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Invocation failed: {e.Message}");
            instanceCache.Remove(methodData.type); // Force recheck next time
            return false;
        }
    }
}