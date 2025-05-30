using System;
using System.Collections;
using System.Collections.Generic;
using LLM.Mcp;

/// <summary>
/// Interface for Python client adapter to handle prediction requests.
/// </summary>
public interface IPythonClientAdapter
{
    /// <summary>
    /// Sends a prediction request to the Python client.
    /// </summary>
    IEnumerator Predict(ModelContext context, Action<Dictionary<string, object>> callback); 
}