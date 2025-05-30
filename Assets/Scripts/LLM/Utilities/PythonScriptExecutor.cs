using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PythonScriptExecutor : Singleton<PythonScriptExecutor>
{
    [Header("PyCharm Configuration")] [SerializeField]
    private string pyCharmPath = @"C:\Program Files\JetBrains\PyCharm 2024.1.4\bin\pycharm64.exe"; // Ruta de PyCharm

    [SerializeField] private string projectPath = @"C:\Users\usuario\PycharmProjects\TFGProject"; // Ruta del proyecto

    [SerializeField]
    private string scriptPath = @"C:\Users\usuario\PycharmProjects\TFGProject\Unity_connection.py"; // Ruta del script

    [SerializeField]
    private string cliCommand = @"C:\Program Files\JetBrains\PyCharm 2024.1.4\bin\pycharm64.exe"; // Ruta del CLI

    [SerializeField] private string pythonArguments = ""; // Argumentos opcionales para el script

    /// <summary>
    ///     Ejecuta el script en PyCharm, abriendo el programa si es necesario.
    /// </summary>
    [ContextMenu("Run Script in PyCharm")]
    public void RunScriptInPyCharm()
    {
        if (!File.Exists(cliCommand))
        {
            Debug.LogError($"No se encontró el CLI de PyCharm en la ruta: {cliCommand}");
            return;
        }

        if (!File.Exists(scriptPath))
        {
            Debug.LogError($"No se encontró el script en la ruta: {scriptPath}");
            return;
        }

        try
        {
            // Verificar si PyCharm ya está abierto
            if (IsPyCharmRunning())
                ExecuteScriptViaPython();
            else
                OpenPyCharmAndRunScript();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error al ejecutar el script en PyCharm: {ex.Message}");
        }
    }

    /// <summary>
    ///     Verifica si PyCharm ya está ejecutándose.
    /// </summary>
    public bool IsPyCharmRunning()
    {
        return Process.GetProcessesByName("pycharm64").Length > 0;
    }

    /// <summary>
    ///     Ejecuta el script en PyCharm utilizando el CLI.
    /// </summary>
    public void ExecuteScriptViaPython()
    {
        var pythonPath = @"C:\Users\usuario\PycharmProjects\TFGProject\.venv\Scripts\python.exe";

        if (!File.Exists(pythonPath))
        {
            Debug.LogError($"Python executable not found at: {pythonPath}");
            return;
        }

        if (!File.Exists(scriptPath))
        {
            Debug.LogError($"Script file not found at: {scriptPath}");
            return;
        }

        try
        {
            var arguments = $"\"{scriptPath}\" {pythonArguments}";

            var startInfo = new ProcessStartInfo
            {
                FileName = pythonPath,
                Arguments = arguments,
                UseShellExecute = true, // Allow process to run independently
                CreateNoWindow = true // Do not show a terminal window
            };

            var process = Process.Start(startInfo);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error executing Python script: {ex.Message}");
        }
    }


    /// <summary>
    ///     Abre PyCharm y ejecuta el script.
    /// </summary>
    public void OpenPyCharmAndRunScript()
    {
        if (!File.Exists(pyCharmPath))
        {
            Debug.LogError($"No se encontró PyCharm en la ruta: {pyCharmPath}");
            return;
        }

        if (!Directory.Exists(projectPath))
        {
            Debug.LogError($"No se encontró el proyecto en la ruta: {projectPath}");
            return;
        }

        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = pyCharmPath,
                Arguments = null!,
                UseShellExecute = false,
                CreateNoWindow = false
            };

            Process.Start(startInfo);
            Debug.Log($"PyCharm abierto con el proyecto y el script: {scriptPath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error al intentar abrir PyCharm y ejecutar el script: {ex.Message}");
        }
    }
}