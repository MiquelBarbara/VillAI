using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightController
{
    private Transform currentTarget;
    private GameObject highlighter;
    
    public HighlightController(GameObject highlighter)
    {
        this.highlighter = highlighter;
    }

    public void Hightlight(Transform target) 
    {
        if (currentTarget == target)
        {
            return;
        }
        currentTarget = target;
        Vector3 position = target.transform.position + Vector3.up * 0.8f;
        Hightlight(position);
        
    }

    private void Hightlight(Vector3 position)
    {
        highlighter.SetActive(true);
        highlighter.transform.position = position;    
    }

    public void Hide()
    {
        currentTarget = null;
        highlighter.SetActive(false);
    }
}