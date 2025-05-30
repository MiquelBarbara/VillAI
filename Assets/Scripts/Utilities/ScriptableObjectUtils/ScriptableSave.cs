using System;
using System.Collections;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Utilities.ScriptableObjectExtensions
{
    public class ScriptableSave : ScriptableObject
    {
        [Header("Save/Load Settings")]
        [SerializeField] protected LoadMode loadMode = LoadMode.Manual;
        [SerializeField] protected SaveMode saveMode = SaveMode.Manual;
        [SerializeField] [Tooltip("Seconds between auto-saves")] 
        protected float saveInterval = 120f;
        
        [Header("Save File Settings")]
        [SerializeField] [HideInInspector] public string subFolder = "";

        [JsonIgnore]
        public string SavePath => GetSavePath();
        public SaveMode GetSaveMode() => saveMode;

        public virtual string GetSavePath()
        {
            return Path.Combine(
                Application.persistentDataPath, 
                subFolder, 
                $"{name}.json"
            );
        }

        public void OnValidate()
        {

            EditorUtility.SetDirty(this);
            
            if (string.IsNullOrEmpty(subFolder))
            {
                subFolder = "Saves";
            }
            
            string directory = Path.GetDirectoryName(SavePath) ?? string.Empty;
            if (Directory.Exists(directory)) return;
            Directory.CreateDirectory(directory);
            Debug.Log($"Created save directory: {directory}");
        }

        public void ChangeSaveFolder(string newFolder)
        {
            if (string.IsNullOrEmpty(newFolder)) return;

            string oldPath = SavePath;
            subFolder = newFolder;
            string newPath = SavePath;

            // Mover el archivo si existe
            if (!File.Exists(oldPath)) return;
            Directory.CreateDirectory(Path.GetDirectoryName(newPath) ?? string.Empty);
            File.Move(oldPath, newPath);
            Debug.Log($"Moved save file to: {newPath}");
        }
        
        public void Save()
        {
            if(!Directory.Exists(Path.GetDirectoryName(SavePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(SavePath));
            }
            string json = JsonUtility.ToJson(this, true);
            string path = SavePath;
            File.WriteAllText(path, json);
        }
        public void Load()
        {
            string path = SavePath;
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                JsonUtility.FromJsonOverwrite(json, this);
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogWarning($"Archivo no encontrado: {path}");
            }
        }
        
        [ContextMenu("Delete Save File")]
        public void DeleteSave()
        {
            if (!File.Exists(SavePath)) return;
            File.Delete(SavePath);
            Debug.Log($"Deleted save file: {SavePath}");
        }
        #if UNITY_EDITOR
        [ContextMenu("Open Save Location")]
        public void OpenSaveLocation()
        {
            EditorUtility.RevealInFinder(SavePath);
        }
        #endif

        #region Mode Handlers
        public void InitializeSaveSystem()
        {
            switch (loadMode)
            {
                case LoadMode.AutoOnStart:
                    Load();
                    break;
                case LoadMode.Manual:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (saveMode)
            {
                case SaveMode.Interval:
                    CoroutineHelper.StartCoroutine(AutoSaveRoutine());
                    break;
                case SaveMode.Manual:
                    break;
                case SaveMode.AutoOnExit:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerator AutoSaveRoutine()
        {
            while (Application.isPlaying)
            {
                yield return new WaitForSeconds(saveInterval);
                Save();
            }
        }

        public void OnApplicationQuit()
        {
            if (saveMode == SaveMode.AutoOnExit)
            {
                Save();
            }
        }
        #endregion
    }
}