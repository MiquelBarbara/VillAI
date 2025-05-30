using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

/// <summary>
/// Factory class for creating and managing bubble UI elements.
/// </summary>
public class BubbleFactory : MonoBehaviour
{
    [SerializeField] private GameObject _bubblePrefab;
    [SerializeField] private Canvas _worldCanvas;
    [SerializeField] private Vector3 _defaultOffset = new Vector3(0, 1.5f, 0);
    
    
    /// <summary>
    /// Creates and displays a bubble UI element at the specified target transform with the given lines of text.
    /// </summary>
    /// <param name="target"> The target transform where the bubble will be displayed.</param>
    /// <param name="lines"> A list of strings representing the lines of text to be displayed in the bubble.</param>
    /// <param name="duration"> The duration for which the bubble will be displayed.</param>
    /// <param name="onComplete"> An optional callback action that will be invoked when the bubble display is complete.</param>
    public void ShowBubble(Transform target, List<string> lines, float duration, Action onComplete = null)
    {
        // Instantiate the bubble prefab and set it as a child of the world canvas.
        var bubbleObj = Instantiate(_bubblePrefab, _worldCanvas.transform);
        var bubble = bubbleObj.GetComponent<Bubble>();
        
        bubble.OnComplete += () => {
            onComplete?.Invoke();
            Destroy(bubbleObj);
        };
        
        bubble.Initialize(lines, target, duration, _defaultOffset);
    }
}