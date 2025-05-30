using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(ItemDatabase))]
public class ItemDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ItemDatabase database = (ItemDatabase)target;

        if (GUILayout.Button("Auto Populate Items"))
        {
            PopulateDatabase(database);
        }
    }

    private void PopulateDatabase(ItemDatabase database)
    {
        // Busca todos los assets de tipo Item dentro de la carpeta Assets.
        string[] guids = AssetDatabase.FindAssets("t:Item", new[] { "Assets" });
        List<Item> foundItems = new List<Item>();

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Item item = AssetDatabase.LoadAssetAtPath<Item>(assetPath);
            if (item != null)
            {
                foundItems.Add(item);
            }
        }

        // Actualiza la lista del ItemDatabase mediante SerializedObject.
        SerializedObject so = new SerializedObject(database);
        SerializedProperty itemsProperty = so.FindProperty("items");
        itemsProperty.ClearArray();
        for (int i = 0; i < foundItems.Count; i++)
        {
            itemsProperty.InsertArrayElementAtIndex(i);
            SerializedProperty element = itemsProperty.GetArrayElementAtIndex(i);
            element.objectReferenceValue = foundItems[i];
        }
        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(database);
    }
}