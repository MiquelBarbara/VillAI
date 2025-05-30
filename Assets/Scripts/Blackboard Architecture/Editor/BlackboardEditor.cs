using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Blackboard_Architecture.Editor
{
    /// <summary>
    /// Custom editor for the <see cref="BlackboardData"/> ScriptableObject.
    /// </summary>
    [CustomEditor(typeof(BlackboardData))]
    public class BlackboardDataEditor : UnityEditor.Editor
    {
        private ReorderableList _entryList;

        /// <summary>
        /// Called when the editor is enabled. Initializes the reorderable list for displaying blackboard entries.
        /// </summary>
        private void OnEnable()
        {
            _entryList = new ReorderableList(serializedObject, serializedObject.FindProperty("entries"), true, true,
                true, true)
            {
                drawHeaderCallback = rect =>
                {
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width * 0.3f, EditorGUIUtility.singleLineHeight),
                        "Key");
                    EditorGUI.LabelField(
                        new Rect(rect.x + rect.width * 0.3f + 10, rect.y, rect.width * 0.3f,
                            EditorGUIUtility.singleLineHeight), "Type");
                    EditorGUI.LabelField(
                        new Rect(rect.x + rect.width * 0.6f + 5, rect.y, rect.width * 0.4f,
                            EditorGUIUtility.singleLineHeight), "Value");
                },
                drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    var element = _entryList.serializedProperty.GetArrayElementAtIndex(index);

                    rect.y += 2;
                    var keyName = element.FindPropertyRelative("keyName");
                    var valueType = element.FindPropertyRelative("valueType");
                    var value = element.FindPropertyRelative("value");

                    var keyNameRect = new Rect(rect.x, rect.y, rect.width * 0.3f, EditorGUIUtility.singleLineHeight);
                    var valueTypeRect = new Rect(rect.x + rect.width * 0.3f, rect.y, rect.width * 0.3f,
                        EditorGUIUtility.singleLineHeight);
                    var valueRect = new Rect(rect.x + rect.width * 0.6f, rect.y, rect.width * 0.4f,
                        EditorGUIUtility.singleLineHeight);

                    EditorGUI.PropertyField(keyNameRect, keyName, GUIContent.none);
                    EditorGUI.PropertyField(valueTypeRect, valueType, GUIContent.none);

                    var selectedType = (AnyValue.ValueType)valueType.enumValueIndex;

                    switch (selectedType)
                    {
                        case AnyValue.ValueType.Int:
                            EditorGUI.PropertyField(valueRect, value.FindPropertyRelative("intValue"), GUIContent.none);
                            break;
                        case AnyValue.ValueType.Float:
                            EditorGUI.PropertyField(valueRect, value.FindPropertyRelative("floatValue"),
                                GUIContent.none);
                            break;
                        case AnyValue.ValueType.Bool:
                            EditorGUI.PropertyField(valueRect, value.FindPropertyRelative("boolValue"),
                                GUIContent.none);
                            break;
                        case AnyValue.ValueType.String:
                            EditorGUI.PropertyField(valueRect, value.FindPropertyRelative("stringValue"),
                                GUIContent.none);
                            break;
                        case AnyValue.ValueType.Vector3:
                            EditorGUI.PropertyField(valueRect, value.FindPropertyRelative("vector3Value"),
                                GUIContent.none);
                            break;
                        case AnyValue.ValueType.GameObject:
                            EditorGUI.PropertyField(valueRect, value.FindPropertyRelative("gameObjectValue"),
                                GUIContent.none);
                            break;
                        case AnyValue.ValueType.Transform:
                            EditorGUI.PropertyField(valueRect, value.FindPropertyRelative("transformValue"),
                                GUIContent.none);
                            break;
                        case AnyValue.ValueType.Behavior:
                            EditorGUI.PropertyField(valueRect, value.FindPropertyRelative("behaviorValue"),
                                GUIContent.none);
                            break;
                        case AnyValue.ValueType.Enum:
                            EditorGUI.PropertyField(valueRect, value.FindPropertyRelative("enumValue"),
                                GUIContent.none);
                            break;
                        case AnyValue.ValueType.Event:
                            EditorGUI.PropertyField(valueRect, value.FindPropertyRelative("eventValue"),
                                GUIContent.none);
                            break;
                        case AnyValue.ValueType.List:
                            EditorGUI.PropertyField(valueRect, value.FindPropertyRelative("listValue"),
                                GUIContent.none);
                            break;
                        case AnyValue.ValueType.Resource:
                            EditorGUI.PropertyField(valueRect, value.FindPropertyRelative("resourceValue"),
                                GUIContent.none);
                            break;
                        default:
                            EditorGUI.LabelField(valueRect, $"Unsupported type: {selectedType}");
                            break;
                    }
                }
            };
        }

        /// <summary>
        /// Draws the custom inspector GUI for the blackboard data.
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            _entryList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
