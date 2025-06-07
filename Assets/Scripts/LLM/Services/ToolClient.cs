// ToolClient.cs

using System;
using UnityEngine;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LLM.Utilities;

/// <summary>
/// ToolClient is a Unity MonoBehaviour that acts as a TCP server to handle tool requests.
/// </summary>
public class ToolClient : MonoBehaviour
{
     [SerializeField] private int port = 8080;
    private TcpListener server;
    private CancellationTokenSource cts;
    private bool isRunning;

    private async void Start()
    {
        cts = new CancellationTokenSource();
        isRunning = true;
        server = new TcpListener(IPAddress.Any, port);
        
        try
        {
            server.Start();
            Debug.Log($"HTTP Tool Server running on port {port}");
            await ListenForRequests(cts.Token);
        }
        catch (OperationCanceledException)
        {
            // Expected during shutdown
        }
        catch (Exception e)
        {
            Debug.LogError($"Server error: {e.Message}");
        }
    }

    /// <summary>
    /// Listens for incoming TCP requests and handles them asynchronously.
    /// </summary>
    /// <param name="ct"></param>
    private async Task ListenForRequests(CancellationToken ct)
    {
        while (isRunning && !ct.IsCancellationRequested)
        {
            try
            {
                TcpClient client = await server.AcceptTcpClientAsync().WithCancellation(ct);
                _ = HandleClient(client, ct); // Fire and forget
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception e)
            {
                Debug.LogError($"Accept error: {e.Message}");
                await Task.Delay(1000, ct); // Wait before retrying
            }
        }
    }

    /// <summary>
    /// Handles an individual TCP client connection, reading the request and sending a response.
    /// </summary>
    /// <param name="client"> The TCP client to handle.</param>
    /// <param name="ct"> Cancellation token to allow graceful shutdown.</param>
    private async Task HandleClient(TcpClient client, CancellationToken ct)
    {
        try
        {
            using (client)
            await using (NetworkStream stream = client.GetStream())
            {
                byte[] buffer = new byte[4096];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, ct);
                string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                string[] requestLines = request.Split('\n');
                string body = requestLines[^1].Trim();

                string response = ProcessRequest(body);
                string httpResponse = $"HTTP/1.1 200 OK\r\nContent-Type: application/json\r\n\r\n{response}";
                byte[] data = Encoding.UTF8.GetBytes(httpResponse);
                await stream.WriteAsync(data, 0, data.Length, ct);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Client handling error: {e.Message}");
        }
    }
    
    /// <summary>
    /// Processes the incoming JSON request, invokes the appropriate tool, and returns a JSON response.
    /// </summary>
    /// <param name="jsonBody"> The JSON body of the request containing tool name and inputs.</param>
    /// <returns> A JSON response indicating success or failure.</returns>
    private string ProcessRequest(string jsonBody)
    {
        try
        {
            var request = JsonParser.FromJson<ToolRequest>(jsonBody);
            if (ToolDispatcher.Instance.TryInvokeTool(request.toolName, request.inputs, out object result))
            {
                return JsonParser.ToJson(new ToolResponse
                {
                    success = true,
                    result = result
                });
            }
            return JsonUtility.ToJson(new ToolResponse { success = false, error = "Tool not found" });
        }
        catch (Exception e)
        {
            return JsonUtility.ToJson(new ToolResponse { success = false, error = e.Message });
        }
    }

    private void OnDestroy()
    {
        isRunning = false;
        cts?.Cancel();
        cts?.Dispose();
        server?.Stop();
    }

    [Serializable] private class ToolRequest
    {
        public string toolName;
        public Dictionary<string, object> inputs;
    }

    [Serializable] private class ToolResponse
    {
        public bool success;
        public object result;
        public string error;
    }
}