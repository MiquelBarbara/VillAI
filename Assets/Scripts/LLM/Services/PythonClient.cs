using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LLM.Adapter;
using LLM.Mcp;
using LLM.Services;
using LLM.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityServiceLocator;

namespace LLM
{
    /// <summary>
    /// Client for communicating with Python prediction services
    /// </summary>
    [RequireComponent(typeof(ToolDispatcher))]
    [RequireComponent(typeof(ToolClient))]
    public class PythonClient : Singleton<PythonClient>
    {
        [SerializeField, Tooltip("URL del servicio de predicción.")]
        private string predictUrl = "http://127.0.0.1:5000/predict";
        
        private IPredictionService _predictionService;
        public string PredictUrl => predictUrl;
        
        const string k_pythonClientName = "Python Client [Connection]";
        
        protected override void Awake()
        {
            base.Awake();
            _predictionService = new PredictionService(
                new PythonClientAdapter(this),
                new PromptGenerator()
            );
            
            ServiceLocator.Global.Register(_predictionService);
        }

        /// <summary>
        /// Executes a Model Context Protocol (MCP) request
        /// </summary>
        /// <param name="context">MCP context containing conversation state</param>
        /// <param name="callback">Callback receiving response data or errors</param>
        /// <returns>Coroutine enumerator</returns>
        public IEnumerator ExecuteMcp(ModelContext context, Action<Dictionary<string, object>> callback)
        {
            var jsonData = JsonParser.ToJson(context);

            using var request = new UnityWebRequest(PredictUrl, "POST");
            var postData = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(postData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            var response = new Dictionary<string, object>();
        
            if (request.result != UnityWebRequest.Result.Success)
            {
                response["error"] = new Dictionary<string, object>
                {
                    ["message"] = request.error,
                    ["code"] = request.responseCode,
                    ["context"] = context
                };
            }
            else
            {
                try
                {
                    response = JsonParser.ToDictionary(request.downloadHandler.text);
                }
                catch (Exception ex)
                {
                    response["error"] = new Dictionary<string, object>
                    {
                        ["message"] = "JSON parse error",
                        ["exception"] = ex.Message,
                        ["rawResponse"] = request.downloadHandler.text
                    };
                }
            }

            callback(response);
        }
#if UNITY_EDITOR
        [MenuItem("GameObject/Python Client/Add Connection")]
        static void AddGlobal() {
            var go = new GameObject(k_pythonClientName, typeof(PythonClient));
        }
        
#endif
    }
}