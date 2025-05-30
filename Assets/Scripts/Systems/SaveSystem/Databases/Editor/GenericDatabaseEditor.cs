namespace Systems.SaveSystem.Databases.Editor
{
#if UNITY_EDITOR
    using UnityEditor;
    using UnityEngine;
    using System.Collections.Generic;

    [CustomEditor(typeof(GenericDatabaseBase), true)]
    public class GenericDatabaseBaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GenericDatabaseBase db = (GenericDatabaseBase)target;

            // Botón para auto-poblar
            if (GUILayout.Button("Auto Populate Database"))
            {
                PopulateDatabase(db);
            }
        }

        private void PopulateDatabase(GenericDatabaseBase db)
        {
            // 1) Obtener el tipo real T
            var typeT = db.GetItemType();

            // 2) Buscar en todo el proyecto (o en una carpeta) los assets de ese tipo
            string[] guids = AssetDatabase.FindAssets("t:" + typeT.Name);

            // Recolectamos en una lista
            List<ScriptableObject> foundItems = new List<ScriptableObject>();
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                ScriptableObject so = AssetDatabase.LoadAssetAtPath(assetPath, typeT) as ScriptableObject;
                if (so != null)
                {
                    foundItems.Add(so);
                }
            }

            // 3) Asignar la lista al database
            db.SetItems(foundItems);

            // 4) Guardar cambios serializados
            EditorUtility.SetDirty(db);
            AssetDatabase.SaveAssets();
            Debug.Log($"Database '{db.name}' updated with {foundItems.Count} {typeT.Name} items.");
        }
    }
#endif

}