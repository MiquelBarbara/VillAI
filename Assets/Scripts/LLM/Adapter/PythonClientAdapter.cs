using System;
using System.Collections;
using System.Collections.Generic;
using LLM.Mcp;

namespace LLM.Adapter
{
    /// <summary>
    /// Adapter implementation for Python client interactions.
    /// </summary>
    public class PythonClientAdapter : IPythonClientAdapter
    {
        // Adapting the PythonClient to the IPythonClientAdapter interface. This allows us to use the PythonClient and change the implementation later if needed.
        private readonly PythonClient _pythonClient;
        
        public PythonClientAdapter(PythonClient pythonClient) => _pythonClient = pythonClient;
        
        
        /// <summary>
        ///  Sends a prediction request to the Python client.
        /// </summary>
        /// <param name="modelContext"> The context containing model information for the prediction.</param>
        /// <param name="callback"> A callback function to handle the prediction result.</param>
        /// <returns> An enumerator for coroutine execution.</returns>
        public IEnumerator Predict(ModelContext modelContext,
            Action<Dictionary<string, object>> callback)
        {
            yield return _pythonClient.ExecuteMcp(modelContext, callback);
        }
    }
}