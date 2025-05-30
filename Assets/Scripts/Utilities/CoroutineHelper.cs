using UnityEngine;
using System.Collections;

public static class CoroutineHelper
{
    private static MonoBehaviour coroutineRunner;
    private static void Initialize()
    {
        if (coroutineRunner != null) return;
        GameObject runnerObject = new GameObject("CoroutineRunner");
        Object.DontDestroyOnLoad(runnerObject);
        coroutineRunner = runnerObject.AddComponent<CoroutineRunner>();
        
        runnerObject.hideFlags = HideFlags.HideInHierarchy;
    }
    
    public static Coroutine StartCoroutine(IEnumerator routine)
    {
        Initialize();
        return coroutineRunner.StartCoroutine(routine);
    }
    
    private class CoroutineRunner : MonoBehaviour { }
}