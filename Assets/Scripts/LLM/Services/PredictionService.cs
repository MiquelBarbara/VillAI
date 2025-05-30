using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using LLM.Adapter;
using LLM.Mcp;
using LLM.Templates;
using LLM.Utilities;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Utilities.ScriptableObjectExtensions;
using Utilities.ScriptableObjectUtils;


namespace LLM.Services
{
    /// <summary>
    /// Interface for prediction services that process template data and return results.
    /// </summary>
    public interface IPredictionService
    {
        /// <summary>
        /// Executes a prediction for a given TemplateData.
        /// </summary>
        /// <param name="templateData">The template data containing input parameters for the prediction.</param>
        /// <returns>An IEnumerator for Unity's coroutine system.</returns>
        IEnumerator Predict(PromptDefinition templateData, DataTransferObject data, string overrideTemplate = null);
    }
    
    /// <summary>
    /// Service responsible for executing predictions and mapping outputs to template properties.
    /// Manages the Model Context Protocol (MCP) communication flow.
    /// </summary>
    public class PredictionService: IPredictionService
    {
        private readonly IPythonClientAdapter _pythonClientAdapter;
        private readonly PromptGenerator _promptGenerator;
        
        public PredictionService(IPythonClientAdapter pythonClientAdapter,
            PromptGenerator promptGenerator)
        {
            _pythonClientAdapter = pythonClientAdapter;
            _promptGenerator    = promptGenerator;
        }

        /// <summary>
        /// Executes a prediction using the Model Context Protocol.
        /// </summary>
        /// <param name="templateData">The template data containing input parameters.</param>
        /// <returns>An IEnumerator for Unity's coroutine system.</returns>
        /// <remarks>
        /// The prediction process follows these steps:
        /// 1. Generates multi-role messages from the template
        /// 2. Calculates context differences using IncrementalContextManager
        /// 3. Creates a ModelContext with messages and tool descriptors
        /// 4. Sends the context to the Python backend and handles the response
        /// </remarks>
        public IEnumerator Predict(PromptDefinition templateData, DataTransferObject data, string overrideTemplate = null)
        {
            if(overrideTemplate != null)
            {
                templateData = ScriptableObjectExtensions.Clone(templateData);
                templateData.templateText = overrideTemplate;
            }
            
            // 1) Generate multi-role messages
            var msgs = _promptGenerator.GenerateMessages(templateData, data);
            
            // Calculate diff and hash
            //var (baseHash, payloadMsgs, isDiff) = _ctxMgr.GetDiff(templateData, msgs);

             // 2) Create MCP context
            var ctx = new ModelContext
            {
                conversationId = Guid.NewGuid().ToString(),
                messages = msgs,
                tools = _promptGenerator.GetToolDescriptors(templateData),
                modelConfig = templateData.modelConfig,
            };

            // 3) Set update callback
            Action<Dictionary<string, object>> onResponse = data.UpdateOutputs;

            // 4) Send prediction request
            yield return _pythonClientAdapter.Predict(ctx, onResponse);
        }
        
    }
}
