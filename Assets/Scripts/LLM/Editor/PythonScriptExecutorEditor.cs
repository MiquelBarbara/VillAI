using UnityEditor;
using UnityEngine;

namespace LLM.Editor
{
    [CustomEditor(typeof(PythonScriptExecutor))]
    public class FlaskServerControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Muestra el contenido predeterminado del Inspector
            DrawDefaultInspector();

            // Agrega un botón personalizado al Inspector
            var scriptExecutor = (PythonScriptExecutor)target;
            if (GUILayout.Button("Abrir Pycharm")) scriptExecutor.OpenPyCharmAndRunScript();

            if (GUILayout.Button("Ejecutar Script Python")) scriptExecutor.ExecuteScriptViaPython();
        }
    }
}