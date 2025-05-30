using UnityEditor;
using UnityEngine;
using System.IO;
using System;
using Utilities.ScriptableObjectExtensions;
using Systems.SaveSystem.Memory;
using NPCs;
using Systems.DialogueSystem.Moods;

public class NPCSetupWizard : EditorWindow
{
    private string characterName = "NewCharacter";
    private GameObject npcPrefab;
    
    [MenuItem("Tools/NPC Setup Wizard")]
    public static void ShowWindow() => GetWindow<NPCSetupWizard>("NPC Setup Wizard");

    void OnGUI()
    {
        GUILayout.Label("NPC Configuration", EditorStyles.boldLabel);
        characterName = EditorGUILayout.TextField("Character Name", characterName);
        npcPrefab = (GameObject)EditorGUILayout.ObjectField("NPC Prefab", npcPrefab, typeof(GameObject), false);

        if (GUILayout.Button("Generate Full NPC Structure"))
        {
            CreateCharacterStructure();
            CreateAllCharacterAssets();
            LinkToResourceLocator();
        }
    }

    private void CreateCharacterStructure()
    {
        string scriptablePath = $"Assets/ScriptableObjects/Characters/{characterName}";
        Directory.CreateDirectory(scriptablePath);
        Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "Characters", characterName));
        AssetDatabase.Refresh();
    }

    private void CreateAllCharacterAssets()
    {
        CreateScriptableObject<ConversationContainer>();
        CreateScriptableObject<RelationshipsContainer>();
        CreateScriptableObject<KnowledgeContainer>();
        CreateScriptableObject<ItemContainer>();
        CreateScriptableObject<Thoughts>();
        CreateScriptableObject<DialogueBag>();
        CreateScriptableObject<MoodContainer>();
        CreateScriptableObject<ComplexCharacterData>(setup: (data) => {
            data.characterName = characterName;
        });
    }

    private void CreateScriptableObject<T>(Action<T> setup = null) where T : ScriptableObject
    {
        string typeName = typeof(T).Name;
        string assetName = $"{characterName}_{typeName}";
        string folderPath = $"Assets/ScriptableObjects/Characters/{characterName}";
        string assetPath = $"{folderPath}/{assetName}.asset";

        // Evitar duplicados
        if (AssetDatabase.LoadAssetAtPath<T>(assetPath) != null) return;

        // Crear asset
        T asset = ScriptableObject.CreateInstance<T>();
        
        // Configuración específica para ScriptableSave
        if (asset is ScriptableSave saveAsset)
        {
            saveAsset.subFolder = $"Characters/{characterName}";
            Debug.Log($"Configurando subFolder para {typeName}: {saveAsset.subFolder}");
        }

        // Configuración adicional personalizada
        setup?.Invoke(asset);

        // Guardar asset
        AssetDatabase.CreateAsset(asset, assetPath);
        Debug.Log($"{typeName} creado en: {assetPath}");
    }

    private void LinkToResourceLocator()
    {
        if (!npcPrefab) return;

        ResourceLocator locator = npcPrefab.GetComponent<ResourceLocator>() ?? npcPrefab.AddComponent<ResourceLocator>();
        string[] guids = AssetDatabase.FindAssets("", new[] {$"Assets/ScriptableObjects/Characters/{characterName}"});

        foreach (string guid in guids)
        {
            ScriptableObject asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(
                AssetDatabase.GUIDToAssetPath(guid)
            );
            
            if (asset && !locator.GetAllResources().Contains(asset))
            {
                locator.RegisterResource(asset);
                Debug.Log($"Vinculando {asset.name} al ResourceLocator");
            }
        }

        EditorUtility.SetDirty(npcPrefab);
        PrefabUtility.SavePrefabAsset(npcPrefab);
    }
}