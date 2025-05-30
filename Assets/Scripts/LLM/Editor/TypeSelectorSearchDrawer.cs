#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TypeSelectorAttribute))]
public class TypeSelectorSearchDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Etiqueta
        float labelWidth = EditorGUIUtility.labelWidth;
        Rect labelRect = new Rect(position.x, position.y, labelWidth, position.height);
        EditorGUI.LabelField(labelRect, label);

        // Campo no editable
        float fieldWidth = position.width - labelWidth - 70;
        Rect fieldRect = new Rect(position.x + labelWidth, position.y, fieldWidth, position.height);

        // Mostrar el nombre corto en lugar del full name
        string fullName = property.stringValue;
        string shortName = ExtractShortName(fullName); 
        if (string.IsNullOrEmpty(shortName)) shortName = "<Ninguno>";

        EditorGUI.LabelField(fieldRect, shortName);

        // Botón "Buscar"
        Rect buttonRect = new Rect(position.x + labelWidth + fieldWidth + 5, position.y, 65, position.height);
        if (GUI.Button(buttonRect, "Buscar"))
        {
            var attr = attribute as TypeSelectorAttribute;
            bool allowLists = attr != null && attr.AllowLists;

            // Abrir la ventana con pestañas
            TypeSearchWindow.ShowWindow(newType =>
            {
                property.stringValue = newType;
                property.serializedObject.ApplyModifiedProperties();
            }, allowLists);
        }
    }

    /// <summary>
    /// Extrae el nombre corto de un FullName (ej: "System.Int32" => "Int32")
    /// o de un List<T> (ej: "System.Collections.Generic.List`1[[System.String]]" => "List<String>").
    /// </summary>
    private string ExtractShortName(string fullName)
    {
        if (string.IsNullOrEmpty(fullName))
            return null;

        // Detectar si es una List<T>
        if (fullName.StartsWith("System.Collections.Generic.List`1[["))
        {
            // Ejemplo: "System.Collections.Generic.List`1[[System.String]]"
            // Queremos extraer la parte de "String"
            int start = fullName.IndexOf("[[") + 2;
            int end = fullName.IndexOf("]]");
            string inner = fullName.Substring(start, end - start); 
            // inner = "System.String"
            var shortInner = ExtractShortName(inner);
            return $"List<{shortInner}>";
        }
        else
        {
            // Si no es lista, simplemente tomamos el último segmento (Type.Name).
            int lastDot = fullName.LastIndexOf('.');
            if (lastDot >= 0 && lastDot < fullName.Length - 1)
                return fullName.Substring(lastDot + 1);
            return fullName; 
        }
    }
}
#endif
