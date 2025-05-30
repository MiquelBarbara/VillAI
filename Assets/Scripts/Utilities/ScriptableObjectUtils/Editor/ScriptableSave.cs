namespace Utilities.ScriptableObjectExtensions.Editor
{
    #if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

[CustomEditor(typeof(ScriptableSave), true)]
public class ScriptableSaveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ScriptableSave saveable = (ScriptableSave)target;
            
        EditorGUILayout.LabelField("Current Save Path", EditorStyles.boldLabel);
        EditorGUILayout.LabelField(saveable.SavePath);
            
        EditorGUILayout.Space(10);
            
        // Selector de carpeta
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Save Folder:", GUILayout.Width(80));
            EditorGUILayout.LabelField(saveable.subFolder, EditorStyles.textField);
                
            if (GUILayout.Button("Change Folder", GUILayout.Width(120)))
            {
                string newPath = EditorUtility.OpenFolderPanel(
                    "Select Save Folder",
                    Path.Combine(Application.persistentDataPath, saveable.subFolder),
                    "");
                    
                if (!string.IsNullOrEmpty(newPath))
                {
                    // Convertir a ruta relativa
                    string relativePath = newPath.Replace(
                        Application.persistentDataPath + Path.DirectorySeparatorChar, 
                        "");
                        
                    if (relativePath != saveable.subFolder)
                    {
                        Undo.RecordObject(saveable, "Change Save Folder");
                        saveable.ChangeSaveFolder(relativePath);
                        EditorUtility.SetDirty(saveable);
                    }
                }
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(15);
        base.OnInspectorGUI();
        
        bool saveExists = File.Exists(saveable.SavePath);
        
        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("Save Management", EditorStyles.boldLabel);
        
        // Primera fila de botones
        EditorGUILayout.BeginHorizontal();
        {
            // Botón Guardar
            if (GUILayout.Button("Save", GUILayout.Height(25)))
            {
                saveable.Save();
                AssetDatabase.Refresh();
            }
            
            // Botón Cargar (deshabilitado si no existe el archivo)
            EditorGUI.BeginDisabledGroup(!saveExists);
            if (GUILayout.Button("Load", GUILayout.Height(25)))
            {
                saveable.Load();
            }
            EditorGUI.EndDisabledGroup();
        }
        EditorGUILayout.EndHorizontal();
        
        // Segunda fila de botones
        EditorGUILayout.BeginHorizontal();
        {
            // Botón Eliminar (deshabilitado si no existe el archivo)
            EditorGUI.BeginDisabledGroup(!saveExists);
            if (GUILayout.Button("Delete", GUILayout.Height(25)))
            {
                if (EditorUtility.DisplayDialog("Delete Save File", 
                    $"Are you sure you want to delete {saveable.SavePath}?", 
                    "Delete", 
                    "Cancel"))
                {
                    saveable.DeleteSave();
                    AssetDatabase.Refresh();
                }
            }
            EditorGUI.EndDisabledGroup();
            
            // Botón Abrir ubicación
            if (GUILayout.Button("Open Location", GUILayout.Height(25)))
            {
                saveable.OpenSaveLocation();
            }
        }
        EditorGUILayout.EndHorizontal();
        
        // Mensaje informativo en Play Mode
        if (Application.isPlaying)
        {
            EditorGUILayout.HelpBox(
                "Game is running - Save/Load operations will affect runtime data", 
                MessageType.Info);
        }
    }
}
#endif
}