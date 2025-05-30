// ToolDefinitionEditor.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace LLM.Tools.Editor
{
    [CustomEditor(typeof(ToolDefinition))]
    public class ToolDefinitionEditor : UnityEditor.Editor
    {
        private struct MethodData
        {
            public string displayName;
            public string toolName;
            public string description;
            public MethodInfo method;
            public Type declaringType;
        }

        private List<MethodData> methods = new List<MethodData>();
        private int selectedIndex = -1;
        private bool needsRefresh = true;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            ToolDefinition tool = (ToolDefinition)target;

            // Refresh methods
            if (needsRefresh || GUILayout.Button("Refresh Methods"))
            {
                FindAllToolMethods();
                needsRefresh = false;
            }

            // Method selection
            int currentIndex = methods.FindIndex(m => 
                m.declaringType?.FullName == tool.methodClassName && 
                m.method.Name == tool.methodName);

            EditorGUI.BeginChangeCheck();
            selectedIndex = EditorGUILayout.Popup("Linked Method", currentIndex,
                methods.Select(m => $"{m.displayName} ({m.toolName})").ToArray());

            if (EditorGUI.EndChangeCheck() && selectedIndex >= 0)
            {
                Undo.RecordObject(target, "Change Tool Method");
                var selected = methods[selectedIndex];
                
                SerializedProperty toolName = serializedObject.FindProperty("toolName");
                toolName.stringValue = selected.toolName;
                
                SerializedProperty description = serializedObject.FindProperty("description");
                description.stringValue = selected.description;
                
                SerializedProperty className = serializedObject.FindProperty("methodClassName");
                className.stringValue = selected.declaringType?.AssemblyQualifiedName;
                
                SerializedProperty methodName = serializedObject.FindProperty("methodName");
                methodName.stringValue = selected.method.Name;
                
                SerializedProperty paramTypes = serializedObject.FindProperty("parameterTypes");
                paramTypes.arraySize = selected.method.GetParameters().Length;
                for (int i = 0; i < paramTypes.arraySize; i++)
                {
                    paramTypes.GetArrayElementAtIndex(i).stringValue = 
                        selected.method.GetParameters()[i].ParameterType.AssemblyQualifiedName;
                }

                serializedObject.ApplyModifiedProperties();
                tool.CacheMethodInfo(); // Pre-cache
            }

            // Validation
            if (!string.IsNullOrEmpty(tool.methodClassName))
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Method Validation", EditorStyles.boldLabel);
                
                if (tool.GetCachedMethod().IsUnityNull())
                {
                    EditorGUILayout.HelpBox("Method not found!", MessageType.Error);
                    if (GUILayout.Button("Clear Invalid Reference"))
                    {
                        ClearInvalidMethod(tool);
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("Valid method reference", MessageType.Info);
                }
            }

            // Default inspector
            EditorGUILayout.Space();
            DrawDefaultInspector();
        }

        private void FindAllToolMethods()
        {
            methods.Clear();
            var nameCount = new Dictionary<string, int>();

            foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(MonoBehaviour))))
                {
                    foreach (var method in type.GetMethods(
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        var attr = method.GetCustomAttribute<ToolMethodAttribute>();
                        if (attr == null) continue;

                        methods.Add(new MethodData
                        {
                            displayName = $"{type.Name}/{method.Name}",
                            toolName = attr.ToolName,
                            description = attr.Description,
                            method = method,
                            declaringType = type
                        });
                    }
                }
            }

            methods = methods.OrderBy(m => m.displayName).ToList();
        }

        private void ClearInvalidMethod(ToolDefinition tool)
        {
            SerializedObject so = new SerializedObject(tool);
            so.FindProperty("methodClassName").stringValue = "";
            so.FindProperty("methodName").stringValue = "";
            so.FindProperty("parameterTypes").arraySize = 0;
            so.ApplyModifiedProperties();
        }
    }
}